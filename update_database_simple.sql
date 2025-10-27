-- EECBET 資料庫結構更新 seeds 文檔（簡化版）
-- 在 SQL Server Management Studio 中執行此腳本

USE EECBET;
GO

-- 步驟 1: 為 members 表添加新欄位
ALTER TABLE members ADD firstname nvarchar(50) NULL;
ALTER TABLE members ADD lastname nvarchar(50) NULL;
ALTER TABLE members ADD gender nvarchar(10) NULL;
ALTER TABLE members ADD birthday datetime2 NULL;
ALTER TABLE members ADD country nvarchar(100) NULL;
ALTER TABLE members ADD points decimal(18,2) NOT NULL DEFAULT 0;
ALTER TABLE members ADD total_bet decimal(18,2) NOT NULL DEFAULT 0;
ALTER TABLE members ADD total_win decimal(18,2) NOT NULL DEFAULT 0;
GO

-- 步驟 2: 為 transactions 表添加新欄位
ALTER TABLE transactions ADD game_name nvarchar(100) NOT NULL DEFAULT '';
ALTER TABLE transactions ADD member_id int NOT NULL DEFAULT 0;
GO

-- 步驟 3: 修改 transactions 表的欄位類型
ALTER TABLE transactions ALTER COLUMN amount decimal(18,2) NOT NULL;
ALTER TABLE transactions ALTER COLUMN points_change decimal(18,2) NOT NULL;
GO

-- 步驟 4: 添加外鍵約束
ALTER TABLE transactions
ADD CONSTRAINT FK_transactions_members_member_id 
FOREIGN KEY (member_id) REFERENCES members(id) ON DELETE CASCADE;
GO

-- 步驟 5: 添加索引
CREATE INDEX IX_transactions_member_id ON transactions(member_id);
GO

PRINT '資料庫結構更新完成！';

