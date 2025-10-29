-- 測試資料庫連接並查詢會員資料
-- 在 Neon 資料庫控制台執行此腳本

-- 1. 檢查 members 表是否存在
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' AND table_name = 'members';

-- 2. 查看當前會員資料（替換實際的會員ID）
SELECT id, username, points, total_bet, total_win 
FROM members 
LIMIT 10;

-- 3. 檢查 bet_records 表是否存在
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public' AND table_name = 'bet_records';

-- 4. 查看最近的投注記錄
SELECT * FROM bet_records 
ORDER BY created_at DESC 
LIMIT 5;

