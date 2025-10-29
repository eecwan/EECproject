-- 在 Neon 資料庫控制台執行此腳本來檢查資料庫狀態

-- 1. 檢查所有資料表
SELECT table_name 
FROM information_schema.tables 
WHERE table_schema = 'public'
ORDER BY table_name;

-- 2. 查看 members 表結構
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'members'
ORDER BY ordinal_position;

-- 3. 查看 bet_records 表結構（如果已創建）
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_name = 'bet_records'
ORDER BY ordinal_position;

-- 4. 查看當前會員資料
SELECT id, username, points, total_bet, total_win, created_at, last_login
FROM members
ORDER BY id;

-- 5. 查看最近的投注記錄（如果表已創建）
-- SELECT * FROM bet_records ORDER BY created_at DESC LIMIT 10;

