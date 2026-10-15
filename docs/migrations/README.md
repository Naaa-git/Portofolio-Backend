# Rollback

Rollback = 2 langkah doang:

1. **Mundurin tag release** → redeploy image versi sebelumnya.
2. **Balikin database** → restore backup yang diambil sebelum deploy versi baru.

Itu aja. Makanya **selalu backup database sebelum deploy**, apapun
migration-nya:

```bash
pg_dump -h localhost -U postgres -d portofolio -F c -f backup_sebelum_deploy_$(date +%Y%m%d_%H%M).dump
```

Kalau perlu rollback:

```bash
# 1. App balik versi lama
docker compose -f docker-compose.prod.yml pull api   # API_VERSION = tag lama
docker compose -f docker-compose.prod.yml up -d api

# 2. DB balik ke backup
pg_restore -h localhost -U postgres -d portofolio -c backup_sebelum_deploy_<tanggal>.dump
```

Nggak perlu mikirin `Down()` migration satu-satu — restore backup udah pasti
balik ke kondisi exact sebelum deploy.

## Catatan soal rollback "sebagian"

Kalau cuma satu release yang bermasalah (misal Release 3 - Experience) dan
Release 1 & 2 aman, restore backup **tetap table/kolom-scoped** (`-t`), bukan
seluruh database — lihat command di atas. Migration terakhir (yang paling
baru diterapkan) yang paling murah buat di-isolasi; migration di tengah
nggak bisa dicabut sendiri kalau migration sesudahnya masih dipertahankan.

Kalau ragu sebuah fitur udah cukup matang, **jangan digabung ke tag release
yang sama** dengan fitur lain yang udah pede — rilis lebih kecil, lebih
sering, kasih jeda buat tes satu-satu, baru lanjut ke yang berikutnya.
