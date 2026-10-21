# 20261021065024_RenameProjectLongDescriptionToContribution

## Apa yang berubah

`Project.LongDescription` (translatable objek, jsonb `{"id":"...","en":"..."}`)
di-rename jadi `Contribution`, dan bentuknya berubah jadi **list** translatable
objek (jsonb `[{"id":"...","en":"..."}, ...]`). Admin sekarang nulis
beberapa poin kontribusi/pencapaian per project (bisa ditambah/dihapus),
bukan 1 paragraf panjang. **Ini bagian yang breaking** — tipe kolom jsonb
objek jadi jsonb array.

## Kenapa

Daripada nulis detail kerjaan di `Experience.Description`, lebih enak
granular per-project — list poin yang bisa di-maintain satu-satu, ditampilin
di publik sebagai bullet list (dots), bukan paragraf naratif.

## SQL `Up()`

```sql
-- 1. Rename kolom (EF cuma deteksi ini — tipe kolom DB-nya sama-sama jsonb,
--    jadi auto-diff nggak nyadar isinya perlu dibungkus jadi array)
ALTER TABLE "Projects" RENAME COLUMN "LongDescription" TO "Contribution";

-- 2. Bungkus data lama jadi array 1 elemen
UPDATE "Projects" SET "Contribution" =
    CASE WHEN "Contribution" = '{}'::jsonb THEN '[]'::jsonb
         ELSE jsonb_build_array("Contribution") END;
```

Project yang sebelumnya punya `LongDescription` terisi **tidak hilang** —
jadi 1 item di list `Contribution` yang baru (1 bullet panjang). Project yang
`LongDescription`-nya kosong (`{}`) jadi list kosong (`[]`), bukan array
berisi objek kosong.

## Batasan `Down()`

```sql
UPDATE "Projects" SET "Contribution" = COALESCE("Contribution"->0, '{}'::jsonb);
ALTER TABLE "Projects" RENAME COLUMN "Contribution" TO "LongDescription";
```

Best-effort: cuma elemen **pertama** array yang dibalikin jadi objek. Kalau
admin udah nambah item ke-2/ke-3 setelah migration ini jalan, item-item itu
**hilang** kalau di-rollback — `Down()` cuma valid buat "batalin perubahan
struktur segera setelah deploy", bukan "balikin ke kondisi lama kapan aja".

## Backup

**Wajib `pg_dump` tabel `Projects` sebelum migration ini dijalankan di
production**:

```bash
pg_dump -h localhost -U postgres -d portofolio -t '"Projects"' -F c -f projects_backup_$(date +%Y%m%d).dump
```
