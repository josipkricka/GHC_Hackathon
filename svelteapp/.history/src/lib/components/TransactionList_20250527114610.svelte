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
    <tbody>
      {#each $transactions as transaction (transaction.trans_num)}
        <tr>
          <td>{formatAmount(transaction.amount)}</td>
          <td>{formatDate(transaction.trans_date)}</td>
          <td>{transaction.trans_time}</td>
          <td>{transaction.category}</td>
          <td>{transaction.first} {transaction.last}</td>
          <td>{transaction.city}</td>
          <td class="actions">
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
    }
    
    .actions {
      flex-direction: column;
      gap: 0.25rem;
    }
    
    .actions-header {
      width: 100px;
    }
  }
</style>
