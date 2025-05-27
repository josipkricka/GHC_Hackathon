/**
 * Helper functions for working with transactions
 */

/**
 * Format a date string to a more readable format
 * @param {string} dateStr - Date string in DD.MM.YYYY format
 * @returns {string} Formatted date
 */
export function formatDate(dateStr) {
  if (!dateStr) return '';
  
  // Parse the date parts
  const parts = dateStr.split('.');
  if (parts.length !== 3) return dateStr;
  
  // Create a date object (note: month is 0-indexed)
  const date = new Date(parts[2], parts[1] - 1, parts[0]);
  
  // Format date in locale format
  return new Intl.DateTimeFormat('en-US').format(date);
}

/**
 * Format amount as currency
 * @param {number} amount - Amount to format
 * @returns {string} Formatted currency
 */
export function formatCurrency(amount) {
  return new Intl.NumberFormat('en-US', { 
    style: 'currency', 
    currency: 'EUR'
  }).format(amount);
}

/**
 * Get unique categories from transactions
 * @param {Array} transactions - List of transactions
 * @returns {Array} Unique categories
 */
export function getUniqueCategories(transactions) {
  const categories = transactions.map(t => t.category);
  return [...new Set(categories)].sort();
}

/**
 * Calculate total amount for transactions
 * @param {Array} transactions - List of transactions
 * @returns {number} Total amount
 */
export function calculateTotal(transactions) {
  return transactions.reduce((sum, t) => sum + Number(t.amount), 0);
}

/**
 * Filter transactions by various criteria
 * @param {Array} transactions - List of transactions
 * @param {Object} filters - Filter criteria
 * @returns {Array} Filtered transactions
 */
export function filterTransactions(transactions, filters = {}) {
  return transactions.filter(t => {
    let match = true;
    
    if (filters.category && t.category !== filters.category) {
      match = false;
    }
    
    if (filters.minAmount && t.amount < filters.minAmount) {
      match = false;
    }
    
    if (filters.maxAmount && t.amount > filters.maxAmount) {
      match = false;
    }
    
    if (filters.search) {
      const search = filters.search.toLowerCase();
      const searchableFields = [
        t.first, 
        t.last, 
        t.city, 
        t.job, 
        t.category
      ];
      
      match = match && searchableFields.some(field => 
        field && field.toLowerCase().includes(search)
      );
    }
    
    return match;
  });
}
