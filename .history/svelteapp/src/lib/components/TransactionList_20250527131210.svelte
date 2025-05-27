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
  {:else}    <div class="card-grid">
      {#each filteredTransactions as transaction (transaction.trans_num)}        <div class="card" 
          in:slide={{ duration: 300 }}
          style="border-left: 4px solid {getCategoryColor(transaction.category)}">
          
          <div class="card-content">
            <div class="card-header">
              <div class="category-amount">
                <div class="category" style="color: {getCategoryColor(transaction.category)};">{transaction.category}</div>
                <div class="amount">{formatAmount(transaction.amount)}</div>
              </div>
              
              <div class="transaction-info">
                <div class="date-time">{formatDate(transaction.trans_date, transaction.trans_time)}</div>
                <div class="trans-id">#{transaction.trans_num}</div>
              </div>
            </div>
            
            <div class="card-body">
              <div class="customer-info">
                <div class="customer-name">{transaction.first} {transaction.last}</div>
                <div class="customer-details">
                  <span class="city">{transaction.city}</span>
                  <span class="job">{transaction.job}</span>
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
          </div>
        </div>
      {/each}
    </div>
  {/if}
</div>

<style>  .controls {
    display: flex;
    justify-content: space-between;
    margin-bottom: 1.5rem;
  }
  
  .search-container {
    flex: 1;
    max-width: 500px;
  }
    .search-input {
    width: 100%;
    padding: 0.75rem 1rem;
    border: 1px solid #ddd;
    border-radius: 0.5rem;
    font-size: 1rem;
    transition: all 0.2s ease;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.05);
    font-weight: 300;
    letter-spacing: 0.3px;
  }
  
  .search-input:focus {
    border-color: #2196F3;
    outline: none;
    box-shadow: 0 2px 8px rgba(33, 150, 243, 0.2);
  }
    .card-grid {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    max-width: 800px;
    margin: 0 auto;
  }
  
  .card {
    background-color: white;
    border-radius: 0.5rem;
    overflow: hidden;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    transition: transform 0.2s ease, box-shadow 0.2s ease;
    width: 100%;
  }
  
  .card:hover {
    transform: translateY(-4px);
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
  }
    .card-content {
    display: flex;
    flex-direction: column;
  }
  
  .card-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    padding: 1rem 1.25rem;
    background-color: #f9f9f9;
    flex-wrap: wrap;
    gap: 0.5rem;
  }
  
  .category-amount {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
  }
  .category {
    font-family: var(--font-heading);
    font-weight: 600;
    font-size: 1.1rem;
    text-transform: capitalize;
    letter-spacing: 0.5px;
  }
  
  .amount {
    font-weight: 700;
    font-size: 1.2rem;
    letter-spacing: -0.5px;
  }
  
  .card-body {
    padding: 1rem 1.25rem;
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-wrap: wrap;
    gap: 1rem;
  }
    .transaction-info {
    display: flex;
    flex-direction: column;
    gap: 0.25rem;
    font-size: 0.875rem;
    color: #666;
    text-align: right;
    font-weight: 300;
  }
    .customer-info {
    margin-right: auto;
  }
  .customer-name {
    font-family: var(--font-heading);
    font-weight: 600;
    font-size: 1.2rem;
    margin-bottom: 0.25rem;
    letter-spacing: -0.5px;
  }
  
  .customer-details {
    display: flex;
    flex-wrap: wrap;
    gap: 0.5rem;
    font-size: 0.875rem;
    color: #666;
    font-weight: 300;
  }
  
  .city:after {
    content: '•';
    margin-left: 0.5rem;
  }
  
  .card-actions {
    display: flex;
    gap: 0.5rem;
  }
    .card-actions button {
    padding: 0.5rem 0.75rem;
    border: none;
    border-radius: 0.25rem;
    cursor: pointer;
    font-size: 0.875rem;
    font-weight: 500;
    display: flex;
    align-items: center;
    justify-content: center;
    gap: 0.5rem;
    transition: all 0.2s ease;
    letter-spacing: 0.5px;
  }
    .edit-btn {
    background-color: #2196F3;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.5rem 0.75rem;
    cursor: pointer;
  }
  
  .delete-btn {
    background-color: #f44336;
    color: white;
    border: none;
    border-radius: 4px;
    padding: 0.5rem 0.75rem;
    cursor: pointer;
  }
  
  .edit-btn:hover {
    background-color: #0b7dda;
  }
  
  .delete-btn:hover {
    background-color: #d32f2f;
  }
  
  .empty-state {
    text-align: center;
    background-color: #f9f9f9;
    padding: 3rem 1rem;
    border-radius: 0.5rem;
    color: #666;
  }
  
  /* Responsive styles */
  @media (max-width: 768px) {
    .card-grid {
      grid-template-columns: 1fr;
    }
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
