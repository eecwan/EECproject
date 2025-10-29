# 重新啟動應用程式

## 問題診斷

發現了關鍵問題：`DrawService` 沒有在 DI 容器中註冊！

## 已經修正

已在 `EECBET/Program.cs` 中添加：
```csharp
// 註冊服務
builder.Services.AddSingleton<DrawHistoryService>();
builder.Services.AddSingleton<DrawService>();
```

## 重新啟動步驟

1. **停止當前運行的應用程式**
   - 在終端按 `Ctrl+C` 停止

2. **重新編譯**
   ```bash
   dotnet build
   ```

3. **重新運行**
   ```bash
   dotnet run
   ```

4. **測試投注功能**
   - 登入會員
   - 進入彩票遊戲進行投注
   - 查看終端是否出現 "投注請求" 日誌
   - 檢查點數是否正確更新

## 預期結果

終端應該會顯示：
```
info: EECBET.Controllers.BetController[0]
      投注請求: MemberId=3, Points=1000.00, TotalAmount=25
info: EECBET.Controllers.BetController[0]
      點數更新: PointsBefore=1000, PointsAfter=1025, ...
```

如果還有問題，請查看瀏覽器控制台的錯誤訊息。
