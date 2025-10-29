-- =============================================
-- 為 SlotRecords 表格添加 MemberId 欄位
-- =============================================

-- 添加 MemberId 欄位（可為空，以支援舊記錄）
ALTER TABLE "SlotRecords" 
ADD COLUMN IF NOT EXISTS "MemberId" INTEGER NULL;

-- 創建索引以提升查詢效能
CREATE INDEX IF NOT EXISTS "IX_SlotRecords_MemberId" ON "SlotRecords" ("MemberId");

-- 可選：如果您想要添加外鍵約束
-- ALTER TABLE "SlotRecords" 
-- ADD CONSTRAINT "FK_SlotRecords_Members_MemberId" 
-- FOREIGN KEY ("MemberId") REFERENCES "Members"("Id") ON DELETE SET NULL;

-- 驗證欄位已添加
SELECT column_name, data_type, is_nullable 
FROM information_schema.columns 
WHERE table_name = 'SlotRecords' 
  AND column_name = 'MemberId';

-- 完成提示
SELECT 'SlotRecords 表格已成功添加 MemberId 欄位！' AS status;

