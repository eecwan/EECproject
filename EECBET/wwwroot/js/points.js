// API 基礎 URL - ASP.NET Core 使用相對路徑
const API_BASE_URL = "/api/points";

// 頁面載入時初始化
document.addEventListener("DOMContentLoaded", function () {
  loadSummary();
  loadTransactions();
  loadCategories();
  initializeMenuSwitching();
});

// 初始化選單切換
function initializeMenuSwitching() {
  const menuItems = document.querySelectorAll(".menu-item");
  const pageContents = document.querySelectorAll(".page-content");

  menuItems.forEach((item) => {
    item.addEventListener("click", function () {
      menuItems.forEach((i) => i.classList.remove("active"));
      pageContents.forEach((p) => p.classList.remove("active"));
      this.classList.add("active");
      const pageName = this.getAttribute("data-page");
      document.getElementById(pageName + "-page").classList.add("active");
    });
  });
}

// 載入統計摘要
async function loadSummary() {
  try {
    const response = await fetch(`${API_BASE_URL}/summary`);
    const result = await response.json();

    if (result.success) {
      const data = result.data;
      document.getElementById("totalBalance").textContent = `NT$ ${formatNumber(
        data.totalBalance
      )}`;

      const todayChange = data.todayChange;
      const todayElement = document.getElementById("todayChange");
      const sign = todayChange >= 0 ? "+" : "";
      todayElement.textContent = `${sign} NT$ ${formatNumber(todayChange)}`;
      todayElement.style.color = todayChange >= 0 ? "#52b788" : "#f44336";

      document.getElementById("totalIncome").textContent = `NT$ ${formatNumber(
        data.totalExpense
      )}`;
      document.getElementById("totalExpense").textContent = `NT$ ${formatNumber(
        data.totalIncome
      )}`;
    }
  } catch (error) {
    console.error("載入統計資料失敗:", error);
  }
}

// 載入交易記錄
async function loadTransactions(filters = {}) {
  const tbody = document.getElementById("transactionsBody");
  tbody.innerHTML =
    '<tr><td colspan="6" style="text-align: center; padding: 40px; color: #999;">載入中...</td></tr>';

  try {
    let url = `${API_BASE_URL}/transactions`;
    const params = new URLSearchParams();

    if (filters.startDate) params.append("startDate", filters.startDate);
    if (filters.endDate) params.append("endDate", filters.endDate);
    if (filters.category && filters.category !== "")
      params.append("category", filters.category);

    if (params.toString()) url += "?" + params.toString();

    const response = await fetch(url);
    const result = await response.json();

    if (result.success && result.data.length > 0) {
      tbody.innerHTML = "";
      result.data.forEach((transaction) => {
        const row = createTransactionRow(transaction);
        tbody.appendChild(row);
      });
    } else {
      tbody.innerHTML =
        '<tr><td colspan="6" style="text-align: center; padding: 40px; color: #999;">無交易記錄</td></tr>';
    }
  } catch (error) {
    console.error("載入交易記錄失敗:", error);
    tbody.innerHTML =
      '<tr><td colspan="6" style="text-align: center; padding: 40px; color: #f44336;">載入失敗</td></tr>';
  }
}

// 創建交易記錄列
function createTransactionRow(transaction) {
  const row = document.createElement("tr");

  const pointsChange = parseFloat(transaction.pointsChange);
  const pointsColor = pointsChange >= 0 ? "#4caf50" : "#f44336";
  const pointsText =
    pointsChange >= 0
      ? `+${formatNumber(Math.abs(pointsChange))}`
      : `-${formatNumber(Math.abs(pointsChange))}`;

  const statusClass =
    transaction.status === "已結單" ? "status-settled" : "status-pending";

  row.innerHTML = `
        <td>${formatDateTime(transaction.transactionTime)}</td>
        <td>${escapeHtml(transaction.category)}</td>
        <td>NT$ ${formatNumber(transaction.amount)}</td>
        <td>${escapeHtml(transaction.type)}</td>
        <td style="color: ${pointsColor}; font-weight: 600;">NT$ ${pointsText}</td>
        <td class="${statusClass}">${escapeHtml(transaction.status)}</td>
    `;

  return row;
}

// 載入遊戲類型選項
async function loadCategories() {
  try {
    const response = await fetch(`${API_BASE_URL}/transactions`);
    const result = await response.json();

    if (result.success) {
      const categories = [...new Set(result.data.map((t) => t.category))];
      const select = document.getElementById("categoryFilter");
      select.innerHTML = '<option value="">全部</option>';

      categories.forEach((category) => {
        const option = document.createElement("option");
        option.value = category;
        option.textContent = category;
        select.appendChild(option);
      });
    }
  } catch (error) {
    console.error("載入遊戲類型失敗:", error);
  }
}

// 篩選交易記錄
function filterTransactions() {
  const filters = {
    startDate: document.getElementById("startDate").value,
    endDate: document.getElementById("endDate").value,
    category: document.getElementById("categoryFilter").value,
  };
  loadTransactions(filters);
}

// 重設篩選
function resetFilters() {
  document.getElementById("startDate").value = "";
  document.getElementById("endDate").value = "";
  document.getElementById("categoryFilter").value = "";
  loadTransactions();
}

// 格式化數字（千分位）
function formatNumber(num) {
  return parseFloat(num).toLocaleString("zh-TW", {
    minimumFractionDigits: 0,
    maximumFractionDigits: 0,
  });
}

// 格式化日期時間
function formatDateTime(dateTimeStr) {
  const date = new Date(dateTimeStr);
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, "0");
  const day = String(date.getDate()).padStart(2, "0");
  const hours = String(date.getHours()).padStart(2, "0");
  const minutes = String(date.getMinutes()).padStart(2, "0");

  return `${year}-${month}-${day} ${hours}:${minutes}`;
}

// HTML 跳脫（防止 XSS）
function escapeHtml(text) {
  const map = {
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#039;",
  };
  return text.replace(/[&<>"']/g, (m) => map[m]);
}
