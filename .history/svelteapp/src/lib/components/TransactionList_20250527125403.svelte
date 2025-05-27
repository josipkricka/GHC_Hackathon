<script>
  import { createEventDispatcher } from 'svelte';
  import { transactionService } from '$lib/services/transaction-service';
  import { fade, slide } from 'svelte/transition';
  
  export let transactions;
  
  const dispatch = createEventDispatcher();
  
  function handleEdit(transaction) {
    dispatch('edit', transaction);
  }
  
  async function handleDelete(transNum) {
    if (confirm('Are you sure you want to delete this transaction?')) {
      await transactionService.delete(transNum);
      // The store will be automatically updated
    }
  }

  // Format functions to improve data display
  function formatDate(dateStr, timeStr) {
    if (!dateStr) return '';
    
    // Create a more readable date format
    const date = new Date(`${dateStr} ${timeStr || '00:00:00'}`);
    return date.toLocaleString('it-IT', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  }
  
  function formatAmount(amount) {
    return new Intl.NumberFormat('it-IT', { style: 'currency', currency: 'EUR' }).format(amount);
  }
  
  // Determine color based on transaction category
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
      // Add more categories as needed
    };
    
    // Default color for undefined categories
    return categoryColors[category?.toLowerCase()] || '#607d8b';
  }
  
  // Search functionality
  let searchQuery = '';
  
  $: filteredTransactions = $transactions.filter(transaction => {
    if (!searchQuery.trim()) return true;
    
    const query = searchQuery.toLowerCase();
    return transaction.trans_num.toLowerCase().includes(query) ||
           transaction.category.toLowerCase().includes(query) ||
           transaction.first.toLowerCase().includes(query) ||
           transaction.last.toLowerCase().includes(query) ||
           transaction.city.toLowerCase().includes(query) ||
           transaction.job.toLowerCase().includes(query);
  });
</script>

<div class="transaction-list">
  <div class="controls">
    <div class="search-container">
      <input 
        type="text" 
        placeholder="Search transactions..." 
        bind:value={searchQuery}
        class="search-input"
      />
    </div>
  </div>

  {#if filteredTransactions.length === 0}
    <div class="empty-state" in:fade>
      <p>No transactions found</p>
    </div>
  {:else}
    <div class="card-grid">
      {#each filteredTransactions as transaction (transaction.trans_num)}
        <div class="card" 
          in:slide={{ duration: 300 }}
          style="border-left: 4px solid {getCategoryColor(transaction.category)}">
          
          <div class="card-header">
            <div class="category">{transaction.category}</div>
            <div class="amount">{formatAmount(transaction.amount)}</div>
          </div>
          
          <div class="card-body">
            <div class="transaction-info">
              <div class="date-time">{formatDate(transaction.trans_date, transaction.trans_time)}</div>
              <div class="trans-id">#{transaction.trans_num}</div>
            </div>
            
            <div class="customer-info">
              <div class="customer-name">{transaction.first} {transaction.last}</div>
              <div class="customer-details">
                <span class="city">{transaction.city}</span>
                <span class="job">{transaction.job}</span>
              </div>
            </div>
          </div>
          
          <div class="card-actions">
            <button class="edit-btn" on:click={() => handleEdit(transaction)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7"></path>
                <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z"></path>
              </svg>
              Edit
            </button>
            <button class="delete-btn" on:click={() => handleDelete(transaction.trans_num)}>
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="3 6 5 6 21 6"></polyline>
                <path d="M19 6v14a2 2 0 0 1-2 2H7a2 2 0 0 1-2-2V6m3 0V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2"></path>
                <line x1="10" y1="11" x2="10" y2="17"></line>
                <line x1="14" y1="11" x2="14" y2="17"></line>
              </svg>
              Delete
            </button>
          </div>
        </div>
      {/each}
    </div>
  {/if}
</div>

<style>
  .transaction-list {
    width: 100%;
    margin-bottom: 2rem;
  }
  
  .edit-btn {
    background-color: #2196F3;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.25rem 0.5rem;
    cursor: pointer;
  }
  
  .delete-btn {
    background-color: #f44336;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.25rem 0.5rem;
    cursor: pointer;
  }
  
  .edit-btn:hover {
    background-color: #0b7dda;
  }
  
  .delete-btn:hover {
    background-color: #d32f2f;
  }
  
  .no-data {
    text-align: center;
    color: #666;
  }
  
  .actions-header {
    width: 150px;
  }
    @media (max-width: 768px) {
    th, td {
      padding: 0.5rem;
      font-size: 0.9rem;
    }
    
    .actions {
      flex-direction: column;
      gap: 0.25rem;
    }
    
    .actions-header {
      width: 100px;
    }
  }
  
  /* Enhanced mobile responsiveness */
  @media (max-width: 576px) {
    .transaction-list {
      margin: -1rem;
    }
    
    table {
      display: block;
    }
    
    thead {
      display: none; /* Hide table headers on mobile */
    }
    
    tbody {
      display: block;
    }
    
    tr {
      display: block;
      margin-bottom: 1rem;
      border: 1px solid #ddd;
      border-radius: 4px;
      box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
      padding: 0.5rem;
      background-color: #fff;
    }
    
    td {
      display: flex;
      padding: 0.35rem 0.5rem;
      text-align: right;
      border-bottom: 1px solid #f0f0f0;
    }
    
    td:before {
      content: attr(data-label);
      font-weight: bold;
      margin-right: auto;
      text-align: left;
    }
    
    td:last-child {
      border-bottom: none;
    }
    
    .actions {
      justify-content: flex-end;
    }
  }
</style>
