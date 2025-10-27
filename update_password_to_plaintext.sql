-- 更新資料庫密碼為明文 - 學生作業用
-- 請在 SQL Server Management Studio 中執行此腳本

USE EECBET;
GO

-- 更新 admin 會員的密碼為明文 "123456"
UPDATE members 
SET password = '123456'
WHERE username = 'admin';

-- 查看更新結果
SELECT id, username, password, email, created_at 
FROM members 
WHERE username = 'admin';

GO

PRINT '密碼已更新為明文！';
PRINT '可以使用以下帳號登入：';
PRINT '帳號: admin';
PRINT '密碼: 123456';

