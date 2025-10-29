# 資料庫設定說明

## 在 Neon PostgreSQL 資料庫中創建 bet_records 表

請在 Neon 資料庫控制台中執行以下 SQL：

```sql
-- 創建 bet_records 表（如果不存在）
CREATE TABLE IF NOT EXISTS bet_records (
    id SERIAL PRIMARY KEY,
    member_id INTEGER NOT NULL,
    game_type VARCHAR(50) NOT NULL,
    issue_no BIGINT NOT NULL,
    bet_numbers TEXT,
    bet_amount NUMERIC(18,2) NOT NULL,
    winning_numbers TEXT,
    win_amount NUMERIC(18,2) NOT NULL,
    created_at TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    result TEXT,
    points_before NUMERIC(18,2) NOT NULL,
    points_after NUMERIC(18,2) NOT NULL
);

-- 創建索引以提升查詢效能
CREATE INDEX IF NOT EXISTS idx_bet_records_member_id ON bet_records(member_id);
CREATE INDEX IF NOT EXISTS idx_bet_records_created_at ON bet_records(created_at DESC);
```

執行完成後，遊戲的點數扣除和中獎增加功能就可以正常運作了。

## 功能說明

1. **投注時自動扣除點數**：當玩家投注時，系統會檢查餘額，扣除投注金額
2. **中獎時自動增加點數**：開獎後如果中獎，獎金會自動加到敢包中
3. **即時更新顯示**：會員專區的餘額每 5 秒自動更新
4. **遊戲記錄保存**：所有投注記錄都會保存到 bet_records 表中

## 測試步驟

1. 登入會員帳號
2. 進入任何彩票遊戲進行投注
3. 查看會員專區，確認餘額已扣除
4. 等待開獎，確認中獎後點數是否正確增加
5. 檢查遊戲記錄表格是否正確顯示

