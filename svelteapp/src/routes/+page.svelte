<script>
  import { transactions } from '$lib/services/transaction-service';
  import { onMount } from 'svelte';
  import TransactionList from '$lib/components/TransactionList.svelte';
  import TransactionForm from '$lib/components/TransactionForm.svelte';
  import Dashboard from '$lib/components/Dashboard.svelte';

  let isAdding = false;
  let editingTransaction = null;
  let pageTitle = 'Transaction Management';
  
  // Track active tab
  let activeTab = 'transactions'; // 'dashboard' or 'transactions'

  function handleAdd() {
    isAdding = true;
    editingTransaction = null;
    // Switch to transactions tab when adding
    activeTab = 'transactions';
  }

  function handleEdit(event) {
    editingTransaction = event.detail;
    isAdding = false;
    // Switch to transactions tab when editing
    activeTab = 'transactions';
  }

  function handleCancel() {
    isAdding = false;
    editingTransaction = null;
  }

  function handleSave() {
    isAdding = false;
    editingTransaction = null;
  }
  
  // Switch between tabs
  function setActiveTab(tab) {
    activeTab = tab;
    // If switching to transactions and currently editing, don't change
    if (tab === 'transactions' && (isAdding || editingTransaction)) {
      return;
    }
    // Reset form state when switching tabs
    isAdding = false;
    editingTransaction = null;
  }
</script>

<svelte:head>
  <title>{pageTitle}</title>
</svelte:head>

<main>
  <div class="container">
    <h1 class="page-title">{pageTitle}</h1>
    
    <!-- Tab Navigation -->
    <div class="tab-navigation">
      <button 
        class="tab-button" 
        class:active={activeTab === 'dashboard'} 
        on:click={() => setActiveTab('dashboard')}
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="tab-icon">
          <rect x="3" y="3" width="7" height="7"></rect>
          <rect x="14" y="3" width="7" height="7"></rect>
          <rect x="14" y="14" width="7" height="7"></rect>
          <rect x="3" y="14" width="7" height="7"></rect>
        </svg>
        Dashboard
      </button>
      <button 
        class="tab-button" 
        class:active={activeTab === 'transactions'} 
        on:click={() => setActiveTab('transactions')}
      >
        <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="tab-icon">
          <line x1="12" y1="1" x2="12" y2="23"></line>
          <path d="M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6"></path>
        </svg>
        Transactions
      </button>
    </div>
    
    {#if activeTab === 'transactions'}
      <div class="header-actions">
        <button class="add-button" on:click={handleAdd}>Add New Transaction</button>
      </div>

      {#if isAdding || editingTransaction}
        <TransactionForm 
          transaction={editingTransaction} 
          isAdding={isAdding} 
          on:cancel={handleCancel}
          on:save={handleSave} />
      {:else}
        <TransactionList {transactions} on:edit={handleEdit} />
      {/if}
    {:else if activeTab === 'dashboard'}
      <Dashboard />
    {/if}
  </div>
</main>

<style>
  .container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 1rem;
  }

  h1.page-title {
    color: #333;
    margin-bottom: 1.5rem;
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 700;
    font-size: 2.2rem;
    letter-spacing: -0.03em;
  }

  .header-actions {
    margin-bottom: 1rem;
  }

  button.add-button {
    background-color: #4CAF50;
    color: white;
    padding: 0.5rem 1rem;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 600;
    transition: background-color 0.2s ease, transform 0.1s ease;
  }

  button.add-button:hover {
    background-color: #45a049;
    transform: translateY(-2px);
  }
  
  button.add-button:active {
    transform: translateY(0);
  }

  main {
    width: 100%;
  }
  
  /* Tab Navigation Styles */
  .tab-navigation {
    display: flex;
    margin-bottom: 1.5rem;
    border-bottom: 1px solid #e0e0e0;
  }
  
  .tab-button {
    padding: 0.75rem 1.5rem;
    background: transparent;
    border: none;
    border-bottom: 3px solid transparent;
    color: #666;
    font-family: 'Space Grotesk', -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif;
    font-weight: 600;
    font-size: 1.1rem;
    cursor: pointer;
    transition: all 0.2s ease;
    margin-right: 0.5rem;
    display: flex;
    align-items: center;
    gap: 0.5rem;
  }
  
  .tab-icon {
    margin-right: 0.25rem;
  }
  
  .tab-button:hover {
    color: #2196F3;
    background-color: rgba(33, 150, 243, 0.05);
  }
  
  .tab-button.active {
    color: #2196F3;
    border-bottom: 3px solid #2196F3;
  }
  @media (max-width: 768px) {
    .container {
      padding: 0.5rem;
    }
    
    button {
      padding: 0.6rem 1rem;
    }
    
    h1 {
      margin-bottom: 1rem;
    }
    
    .tab-button {
      padding: 0.6rem 1rem;
      font-size: 1rem;
    }
  }
  
  @media (max-width: 576px) {
    .container {
      padding: 0.5rem;
      width: 100%;
    }
    
    main {
      padding: 0;
    }
    
    .header-actions {
      display: flex;
      justify-content: stretch;
    }
    
    button {
      flex: 1;
      padding: 0.75rem;
      font-size: 1rem;
    }
    
    .tab-navigation {
      display: flex;
      width: 100%;
    }
    
    .tab-button {
      flex: 1;
      text-align: center;
      margin-right: 0;
      justify-content: center;
    }
  }
</style>
