<script>
  import { onMount, onDestroy } from 'svelte';
  import { transactions } from '$lib/services/transaction-service';
  
  // Statistics data
  let stats = {
    totalTransactions: 0,
    totalAmount: 0,
    transactionsByCategory: {},
    totalVolume: 0,
    averageTransaction: 0
  };
  
  let transactionData = [];
  
  // Subscribe to transactions changes
  const unsubscribe = transactions.subscribe(value => {
    transactionData = value || [];
    calculateStats();
  });
  
  // Cleanup subscriptions on component destroy
  onDestroy(() => {
    unsubscribe();
  });
  
  // Calculate all statistics
  function calculateStats() {
    if (!transactionData || transactionData.length === 0) {
      return;
    }
    
    // Total transactions
    stats.totalTransactions = transactionData.length;
    
    // Total amount
    stats.totalAmount = transactionData.reduce((sum, t) => sum + parseFloat(t.amount), 0);
    
    // Transactions by category
    const categoryMap = new Map();
    transactionData.forEach(t => {
      const category = t.category || 'Other';
      if (!categoryMap.has(category)) {
        categoryMap.set(category, {
          count: 0,
          total: 0
        });
      }
      
      const categoryData = categoryMap.get(category);
      categoryData.count += 1;
      categoryData.total += parseFloat(t.amount);
    });
    
    stats.transactionsByCategory = Object.fromEntries(categoryMap);
    
    // Total volume (sum of absolute values)
    stats.totalVolume = transactionData.reduce((sum, t) => sum + Math.abs(parseFloat(t.amount)), 0);
    
    // Average transaction
    stats.averageTransaction = stats.totalAmount / stats.totalTransactions;
  }
  
  // Format amount as currency
  function formatAmount(amount) {
    return new Intl.NumberFormat('it-IT', { style: 'currency', currency: 'EUR' }).format(amount);
  }
  
  // Get color for category
  function getCategoryColor(category) {
    const categoryColors = {
      'food': '#4caf50',
      'groceries': '#8bc34a',
      'shopping': '#2196f3',
      'entertainment': '#9c27b0',
      'travel': '#ff9800',
      'bills': '#f44336',
      'health': '#00bcd4',
      'education': '#3f51b5',
      'home': '#795548',
      'clothing': '#607d8b'
    };
    
    return categoryColors[category?.toLowerCase()] || '#607d8b';
  }
  
  onMount(() => {
    calculateStats();
  });
</script>

<div class="dashboard">
  <h2 class="dashboard-title">Financial Dashboard</h2>
  
  {#if transactionData.length === 0}
    <div class="empty-state">
      <p>No transactions available to display statistics</p>
      <p class="hint">Add some transactions to see statistics</p>
    </div>
  {:else}
    <div class="stat-cards">
      <!-- Total Transactions -->
      <div class="stat-card">
        <div class="stat-header">
          <h3>Total Transactions</h3>
        </div>
        <div class="stat-value">{stats.totalTransactions}</div>
      </div>
      
      <!-- Total Amount -->
      <div class="stat-card">
        <div class="stat-header">
          <h3>Total Amount</h3>
        </div>
        <div class="stat-value">{formatAmount(stats.totalAmount)}</div>
      </div>
      
      <!-- Total Volume -->
      <div class="stat-card">
        <div class="stat-header">
          <h3>Total Volume</h3>
        </div>
        <div class="stat-value">{formatAmount(stats.totalVolume)}</div>
      </div>
      
      <!-- Average Transaction -->
      <div class="stat-card">
        <div class="stat-header">
          <h3>Average Transaction</h3>
        </div>
        <div class="stat-value">{formatAmount(stats.averageTransaction)}</div>
      </div>
    </div>
    
    <!-- Transactions by Category -->
    <div class="category-section">
      <h3 class="section-title">Transactions by Category</h3>
      
      <div class="category-grid">
        {#each Object.entries(stats.transactionsByCategory) as [category, data]}
          <div class="category-card" style="border-left: 4px solid {getCategoryColor(category)}">
            <div class="category-name" style="color: {getCategoryColor(category)}">
              {category}
            </div>
            <div class="category-stats">
              <div class="category-count">
                <span class="label">Count:</span>
                <span class="value">{data.count}</span>
              </div>
              <div class="category-amount">
                <span class="label">Total:</span>
                <span class="value">{formatAmount(data.total)}</span>
              </div>
            </div>
          </div>
        {/each}
      </div>
    </div>
  {/if}
</div>

<style>
  .dashboard {
    max-width: 1000px;
    margin: 0 auto;
    padding: 1.5rem 0;
  }
  
  .dashboard-title {
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 700;
    font-size: 1.8rem;
    margin-bottom: 1.5rem;
    color: #333;
  }
  
  .empty-state {
    background-color: #f9f9f9;
    border-radius: 8px;
    padding: 2rem;
    text-align: center;
    margin: 2rem 0;
  }
  
  .empty-state p {
    margin: 0;
    color: #666;
    font-family: 'Outfit', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-size: 1.1rem;
  }
  
  .empty-state .hint {
    font-size: 0.9rem;
    color: #888;
    margin-top: 0.5rem;
  }
  
  .stat-cards {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    gap: 1rem;
    margin-bottom: 2rem;
  }
  
  .stat-card {
    background-color: white;
    border-radius: 8px;
    padding: 1.25rem;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
  }
  
  .stat-card:hover {
    transform: translateY(-4px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
  
  .stat-header h3 {
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 600;
    font-size: 1rem;
    color: #666;
    margin: 0 0 0.75rem 0;
  }
  
  .stat-value {
    font-family: 'Outfit', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 700;
    font-size: 1.75rem;
    color: #333;
  }
  
  .section-title {
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 600;
    font-size: 1.4rem;
    margin: 1.5rem 0 1rem 0;
    color: #333;
  }
  
  .category-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
    gap: 1rem;
  }
  
  .category-card {
    background-color: white;
    border-radius: 6px;
    padding: 1rem;
    box-shadow: 0 2px 6px rgba(0, 0, 0, 0.08);
    transition: transform 0.2s ease;
  }
  
  .category-card:hover {
    transform: translateY(-2px);
  }
  
  .category-name {
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 600;
    font-size: 1.1rem;
    margin-bottom: 0.5rem;
  }
  
  .category-stats {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
  }
  
  .category-count, .category-amount {
    display: flex;
    justify-content: space-between;
    font-family: 'Outfit', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
  }
  
  .label {
    color: #666;
    font-size: 0.9rem;
  }
  
  .value {
    font-weight: 600;
    color: #333;
  }
  
  @media (max-width: 768px) {
    .stat-cards {
      grid-template-columns: repeat(2, 1fr);
    }
    
    .category-grid {
      grid-template-columns: 1fr;
    }
  }
  
  @media (max-width: 480px) {
    .stat-cards {
      grid-template-columns: 1fr;
    }
  }
</style>
