## Semantic Kernel 請假助理

這段程式碼是用 C# 寫的，它使用了 OpenAI 的 API 來創建一個 AI 聊天機器人。這個機器人可以與使用者進行對話，並可以直接呼叫一些特定的函數來執行特定的任務。  

我們首先設定了 Azure OpenAI 的相關資訊，包括模型部署名稱、API 端點和 API 金鑰。然後，我們建立了一個新的 Kernel 物件，並將 LeaveRequestPlugin 加入到該 Kernel 中。

接著，我們建立了一個 ChatHistory 物件，並加入了一條系統訊息，這條訊息說明了這個程式的功能。

然後，我們從 Kernel 中取得了 IChatCompletionService，並開始了一個對話循環。在這個循環中，我們讀取使用者的輸入，並將其加入到聊天歷史中。然後，我們設定了 OpenAIPromptExecutionSettings，並使用 chatCompletionService 從 AI 獲取回應。最後，我們將 AI 的回應加入到聊天歷史中，並再次讀取使用者的輸入。

LeaveRequestPlugin 類別包含了三個方法，分別是 GetCurrentDate、GetLeaveRecordAmount 和 LeaveRequest。這些方法都被標記為 KernelFunction，表示它們可以被 Kernel 調用。

GetCurrentDate 方法返回當前的日期和時間，GetLeaveRecordAmount 方法返回指定員工的請假天數，LeaveRequest 方法則用於處理請假請求。

運行後，你會發現用戶可以透過這個自然語言對話機器人，完成請假與查詢的功能。
