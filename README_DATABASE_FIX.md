# 資料庫點數更新問題解決指南

## 問題診斷

如果點數增減沒有正確顯示，請按照以下步驟檢查：

### 步驟 1: 檢查資料庫連接

請在 Neon 資料庫控制台執行 `check_database.sql` 來檢查：
1. bet_records 表是否已創建
2. members 表的資料是否正確

### 步驟 2: 創建 bet_records 表

如果 bet_records 表不存在，請執行：

```sql
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

CREATE INDEX IF NOT EXISTS idx_bet_records_member_id ON bet_records(member_id);
CREATE INDEX IF NOT EXISTS idx_bet_records_created_at ON bet_records(created_at DESC);
```

### 步驟 3: 檢查日志輸出

運行應用程式後，在終端查看日誌輸出，應該會看到：
- "投注請求: MemberId=..., Points=..., TotalAmount=..."
- "點數更新: PointsBefore=..., BetAmount=..., PointsAfter=..."
- "投注記錄已保存: ..."
- "獲取餘額: MemberId=..., Points=..."

### 步驟 4: 測試流程

1. 登入會員帳號
2. 進入任何彩票遊戲進行投注（例如投注 25 元）
3. 等待開獎
4. 查看終端日誌，確認：
   - 投注請求是否收到
   - 點數是否正確計算
   - SQL 更新是否執行
   - 投注記錄是否保存
5. 查看會員專區，確認：
   - 餘額是否更新
   - 遊戲記錄是否顯示
   - 總投注額和總中獎額是否更新

### 步驟 5: 手動驗證資料庫

在 Neon 控制台執行：

```sql
-- 查看會員當前點數
SELECT id, username, points, total_bet, total_win 
FROM members 
WHERE id = YOUR_MEMBER_ID;

-- 查看投注記錄
SELECT * FROM bet_records 
WHERE member_id = YOUR_MEMBER_ID
ORDER BY created_at DESC;
```

## 常見問題

### 問題 1: bet_records 表不存在
**解決方法**: 執行上述 CREATE TABLE 語句

### 問題 2: 點數沒有正確更新
**可能原因**: 
- bet_records 表沒有正確創建
- SQL UPDATE 語句執行失敗

**檢查方法**: 查看終端日誌中的 "點數更新" 記錄

### 問題 3: 會員專區顯示舊資料
**可能原因**: 前端快取或 API 沒有正確調用

**解決方法**: 
- 按 F5 強制刷新頁面
- 檢查瀏覽器控制台的 Network 標籤
- 確認 `/Member/GetBalance` API 返回的資料

