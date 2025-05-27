<script>
  import { createEventDispatcher } from 'svelte';
  import { transactionService } from '$lib/services/transaction-service';
  
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
  function formatDate(dateStr) {
    return dateStr;  // You can enhance formatting if needed
  }
  
  function formatAmount(amount) {
    return new Intl.NumberFormat('it-IT', { style: 'currency', currency: 'EUR' }).format(amount);
  }
</script>

<div class="transaction-list">
  <table>
    <thead>
      <tr>
        <th>Amount</th>
        <th>Date</th>
        <th>Time</th>
        <th>Category</th>
        <th>Name</th>
        <th>City</th>
        <th class="actions-header">Actions</th>
      </tr>
    </thead>
    <tbody>      {#each $transactions as transaction (transaction.trans_num)}
        <tr>
          <td data-label="Amount">{formatAmount(transaction.amount)}</td>
          <td data-label="Date">{formatDate(transaction.trans_date)}</td>
          <td data-label="Time">{transaction.trans_time}</td>
          <td data-label="Category">{transaction.category}</td>
          <td data-label="Name">{transaction.first} {transaction.last}</td>
          <td data-label="City">{transaction.city}</td>
          <td data-label="Actions" class="actions">
            <button class="edit-btn" on:click={() => handleEdit(transaction)}>Edit</button>
            <button class="delete-btn" on:click={() => handleDelete(transaction.trans_num)}>Delete</button>
          </td>
        </tr>
      {/each}
      
      {#if $transactions.length === 0}
        <tr>
          <td colspan="7" class="no-data">No transactions found</td>
        </tr>
      {/if}
    </tbody>
  </table>
</div>

<style>
  .transaction-list {
    overflow-x: auto;
    margin-bottom: 1rem;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
  }
  
  th, td {
    padding: 0.75rem;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }
  
  th {
    background-color: #f2f2f2;
    font-weight: bold;
  }
  
  tr:hover {
    background-color: #f5f5f5;
  }
  
  .actions {
    display: flex;
    gap: 0.5rem;
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
