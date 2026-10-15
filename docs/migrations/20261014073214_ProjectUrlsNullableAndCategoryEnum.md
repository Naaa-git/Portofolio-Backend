# 20261014073214_ProjectUrlsNullableAndCategoryEnum

## Apa yang berubah

Dua perubahan pada `Project`, dibundel dalam satu migration karena EF Core
men-diff seluruh model sekaligus waktu migration-nya di-generate:

1. `GithubUrl` / `DemoUrl` — dari required (`text, nullable: false`) jadi
   nullable. **Tidak breaking** — cuma melonggarkan constraint, nggak ada
   data yang diubah/hilang.
2. `Category` — dari free-text translatable (`jsonb`, `{"id":"...","en":"..."}`)
   jadi enum tetap `ProjectCategory { Personal, Freelance, Work }` (disimpan
   sebagai `integer`). **Ini bagian yang breaking.**

## Kenapa

Admin nggak tau mau isi apa di field Category yang free-text — diganti jadi
pilihan tetap (dropdown) biar konsisten. GithubUrl/DemoUrl jadi nullable
karena beberapa project confidential, nggak punya link publik sama sekali.

## SQL `Up()` — bagian Category

```sql
ALTER TABLE "Projects" ALTER COLUMN "Category" TYPE integer USING 2;
```

5 project yang udah ada (seed data: Enterprise, Produktivitas ×2, Analitik,
Security) **semuanya di-set ke `Work` (2)** sebagai default aman — teks
kategori lama nggak bisa dipetakan otomatis ke salah satu dari 3 nilai enum
baru. **Perlu tindakan manual setelah deploy**: re-kategorisasi ke-5 project
itu lewat dropdown di admin panel kalau kategorinya bukan "Work".

## Batasan `Down()`

```sql
ALTER TABLE "Projects" ALTER COLUMN "Category" TYPE jsonb USING '{}'::jsonb;
```

Best-effort doang — teks kategori asli **sudah hilang** begitu `Up()` jalan
(baik rollback maupun nggak), jadi `Down()` cuma ngembaliin bentuk kolom
(jsonb kosong), bukan isi aslinya. Rollback ini cuma valid dipakai buat
"batalin perubahan struktur", bukan "balikin data kategori lama".

## Backup

**Wajib `pg_dump` tabel `Projects` di VPS sebelum migration ini dijalankan
di production**, biar ada cara balik ke kondisi exact semula kalau
`Down()`-nya nggak cukup:

```bash
pg_dump -h localhost -U postgres -d portofolio -t '"Projects"' -F c -f projects_backup_$(date +%Y%m%d).dump
```
