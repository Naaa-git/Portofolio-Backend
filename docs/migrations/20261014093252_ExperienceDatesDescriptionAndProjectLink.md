# 20261014093252_ExperienceDatesDescriptionAndProjectLink

## Apa yang berubah

Tiga perubahan pada `Experience`/`Project`, dibundel jadi satu migration
(EF Core men-diff seluruh model sekaligus pas di-generate):

1. `Experience.Period` (text bebas, misal `"Jul 2024 – Present"`) → `StartDate`
   (date) + `EndDate` (date, nullable — null = masih berjalan, dipasangkan
   sama `Current`). **Breaking.**
2. `Experience.Description` — dari list bullet point jadi satu field teks.
   **Breaking.** EF Core sendiri **nggak ngedeteksi ini sebagai perubahan**
   (tipe kolom Postgres-nya tetep `jsonb` baik sebelum maupun sesudah, cuma
   shape JSON di dalemnya yang beda — array vs object), jadi transform-nya
   ditulis manual, bukan hasil auto-generate EF.
3. `Project.ExperienceId` (FK baru, nullable) — satu project bisa "dikerjain
   pas kerja di experience X". **Tidak breaking** (kolom baru, default null).

## Kenapa

Period jadi date picker biar nggak ada lagi typo format tanggal manual.
Description disederhanain jadi satu field biar nggak keribetan nambah bullet
satu-satu. Project↔Experience link biar bisa nunjukin "project ini dikerjain
pas kerja di perusahaan X" di halaman publik.

## SQL `Up()` — bagian breaking

**Period → StartDate/EndDate** (parsing, bukan reset ke kosong):

```sql
UPDATE "Experiences" SET
  "StartDate" = CASE WHEN split_part("Period", ' – ', 1) ~ '^[A-Za-z]{3} \d{4}$'
                 THEN to_date(split_part("Period", ' – ', 1), 'Mon YYYY') ELSE '2026-01-01' END,
  "EndDate" = CASE WHEN "Current" THEN NULL
               WHEN split_part("Period", ' – ', 2) ~ '^[A-Za-z]{3} \d{4}$'
                 THEN to_date(split_part("Period", ' – ', 2), 'Mon YYYY') ELSE '2026-01-01' END;
```

Format yang dikenali: `"<Mon> <YYYY> – <Mon> <YYYY>"` atau `"<Mon> <YYYY> – Present"`.
Apapun yang nggak cocok sama format itu (termasuk kalau Period kosong/aneh)
di-default ke **1 Januari 2026** — dikonfirmasi langsung sama project owner
sebagai fallback yang disengaja, bukan bug. **Perlu dicek manual setelah
deploy**: experience mana aja yang tanggalnya jadi 2026-01-01 berarti nggak
ke-parse, perlu dibenerin lewat date picker di admin panel.

**Description → single field** (gabung, bukan buang):

```sql
UPDATE "Experiences"
SET "Description" = jsonb_build_object(
  'id', (SELECT string_agg(elem->>'id', E'\n') FROM jsonb_array_elements("Description") elem),
  'en', (SELECT string_agg(elem->>'en', E'\n') FROM jsonb_array_elements("Description") elem)
);
```

Semua bullet lama digabung jadi satu paragraf (dipisah baris baru per
bullet), **nggak ada yang dihapus** — tinggal dirapihin manual sekali lewat
admin panel kalau hasil gabungannya kurang pas secara kalimat.

## Batasan `Down()`

- **Description**: `Down()` cuma bungkus teks yang udah gabung jadi array
  1-elemen (`jsonb_build_array`) — batas bullet asli udah nggak bisa
  dipulihin, sama kayak limitasi yang udah dicatat di migration Category.
- **Period**: `Down()` rekonstruksi ulang dari `StartDate`/`EndDate` jadi
  string format yang sama — tapi ini **cuma equivalent, bukan teks asli**.
  Kalau Period aslinya punya typo/format aneh, itu nggak akan kebalik sama
  persis.
- Rollback ini valid dipakai buat "batalin perubahan struktur", bukan
  "balikin ke teks/bullet asli yang persis sama".

## Backup

**Wajib `pg_dump` tabel `Experiences` dan `Projects` di VPS sebelum migration
ini jalan di production:**

```bash
pg_dump -h localhost -U postgres -d portofolio -t '"Experiences"' -t '"Projects"' -F c -f experiences_projects_backup_$(date +%Y%m%d).dump
```
