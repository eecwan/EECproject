-- 為 PostgreSQL/Neon 創建 bet_records 表
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

