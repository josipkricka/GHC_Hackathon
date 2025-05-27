<script>
  import { transactions } from '$lib/services/transaction-service';
  import { onMount } from 'svelte';
  import TransactionList from '$lib/components/TransactionList.svelte';
  import TransactionForm from '$lib/components/TransactionForm.svelte';

  let isAdding = false;
  let editingTransaction = null;
  let pageTitle = 'Transaction Management';

  function handleAdd() {
    isAdding = true;
    editingTransaction = null;
  }

  function handleEdit(event) {
    editingTransaction = event.detail;
    isAdding = false;
  }

  function handleCancel() {
    isAdding = false;
    editingTransaction = null;
  }

  function handleSave() {
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
    font-family: var(--font-heading);
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
    font-family: var(--font-heading);
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
  }
</style>
