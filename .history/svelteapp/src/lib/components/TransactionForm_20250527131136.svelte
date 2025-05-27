<script>
  import { createEventDispatcher } from 'svelte';
  import { transactionService } from '$lib/services/transaction-service';
  import { Transaction } from '$lib/models/transaction';
  
  export let transaction = null;
  export let isAdding = true;
  
  const dispatch = createEventDispatcher();
  
  // Form data is stored here
  let formData = isAdding 
    ? new Transaction() 
    : { ...transaction };
  
  // Categories for the dropdown
  const categories = [
    'grocery_pos',
    'grocery_net',
    'misc_net',
    'misc_pos',
    'entertainment',
    'gas_transport',
    'shopping_net',
    'shopping_pos'
  ];
  
  // Gender options
  const genders = ['M', 'F'];
  
  // Handle form submission
  async function handleSubmit() {
    try {
      if (isAdding) {
        // Generate a simple ID for the new transaction
        formData.trans_num = crypto.randomUUID().replace(/-/g, '');
        await transactionService.create(formData);
      } else {
        await transactionService.update(formData);
      }
      dispatch('save');
    } catch (error) {
      console.error('Error saving transaction:', error);
      alert('Failed to save transaction');
    }
  }
  
  // Handle cancellation
  function handleCancel() {
    dispatch('cancel');
  }
</script>

<div class="transaction-form">
  <h2>{isAdding ? 'Add New' : 'Edit'} Transaction</h2>
  
  <form on:submit|preventDefault={handleSubmit}>
    <div class="form-group">
      <label for="amount">Amount</label>
      <input 
        type="number" 
        id="amount" 
        bind:value={formData.amount} 
        required
      />
    </div>
    
    <div class="form-group">
      <label for="trans_date">Date</label>
      <input 
        type="text" 
        id="trans_date" 
        bind:value={formData.trans_date} 
        placeholder="DD.MM.YYYY"
        required
      />
    </div>
    
    <div class="form-group">
      <label for="trans_time">Time</label>
      <input 
        type="text" 
        id="trans_time" 
        bind:value={formData.trans_time}
        placeholder="HH:MM:SS"
        required
      />
    </div>
    
    <div class="form-group">
      <label for="category">Category</label>
      <select id="category" bind:value={formData.category} required>
        <option value="">Select a category</option>
        {#each categories as category}
          <option value={category}>{category}</option>
        {/each}
      </select>
    </div>
    
    <div class="form-row">
      <div class="form-group">
        <label for="first">First Name</label>
        <input 
          type="text" 
          id="first" 
          bind:value={formData.first}
          required
        />
      </div>
      
      <div class="form-group">
        <label for="last">Last Name</label>
        <input 
          type="text" 
          id="last" 
          bind:value={formData.last}
          required
        />
      </div>
    </div>
    
    <div class="form-row">
      <div class="form-group">
        <label for="gender">Gender</label>
        <select id="gender" bind:value={formData.gender} required>
          <option value="">Select gender</option>
          {#each genders as gender}
            <option value={gender}>{gender}</option>
          {/each}
        </select>
      </div>
      
      <div class="form-group">
        <label for="dob">Date of Birth</label>
        <input 
          type="text" 
          id="dob" 
          bind:value={formData.dob}
          placeholder="DD.MM.YYYY"
          required
        />
      </div>
    </div>
    
    <div class="form-group">
      <label for="city">City</label>
      <input 
        type="text" 
        id="city" 
        bind:value={formData.city}
        required
      />
    </div>
    
    <div class="form-group">
      <label for="job">Job</label>
      <input 
        type="text" 
        id="job" 
        bind:value={formData.job}
        required
      />
    </div>
    
    <div class="form-actions">
      <button type="button" class="cancel-btn" on:click={handleCancel}>Cancel</button>
      <button type="submit" class="save-btn">Save</button>
    </div>
  </form>
</div>

<style>
  .transaction-form {
    background-color: #f9f9f9;
    padding: 1.5rem;
    border-radius: 8px;
    margin-bottom: 1rem;
  }
  
  h2 {
    margin-top: 0;
    margin-bottom: 1.5rem;
    color: #333;
    font-family: var(--font-heading);
    font-weight: 600;
    letter-spacing: -0.02em;
  }
  
  .form-group {
    margin-bottom: 1rem;
    width: 100%;
  }
  
  .form-row {
    display: flex;
    gap: 1rem;
    margin-bottom: 1rem;
  }
  
  label {
    display: block;
    margin-bottom: 0.5rem;
    font-weight: bold;
    color: #555;
  }
  
  input, select {
    width: 100%;
    padding: 0.5rem;
    border: 1px solid #ddd;
    border-radius: 4px;
    font-size: 1rem;
  }
  
  .form-actions {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
  }
    .cancel-btn {
    background-color: #ccc;
    color: #333;
    border: none;
    padding: 0.5rem 1.5rem;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
    font-family: var(--font-heading);
    font-weight: 500;
    transition: all 0.2s ease;
  }
  
  .save-btn {
    background-color: #4CAF50;
    color: white;
    border: none;
    padding: 0.5rem 1.5rem;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
    font-family: var(--font-heading);
    font-weight: 500;
    transition: all 0.2s ease;
  }
    .cancel-btn:hover {
    background-color: #bbb;
    transform: translateY(-2px);
  }
  
  .save-btn:hover {
    background-color: #45a049;
    transform: translateY(-2px);
  }
  
  .cancel-btn:active, .save-btn:active {
    transform: translateY(0);
  }
    @media (max-width: 768px) {
    .form-row {
      flex-direction: column;
      gap: 0;
    }
    
    .transaction-form {
      padding: 1rem;
    }
    
    .form-actions {
      flex-direction: column-reverse; /* Put the Save button on top on mobile */
      gap: 0.75rem;
    }
    
    .save-btn, .cancel-btn {
      width: 100%; /* Full width buttons on mobile */
      padding: 0.75rem;
    }
  }
  
  /* Further enhance for very small screens */
  @media (max-width: 576px) {
    .transaction-form {
      padding: 0.75rem;
      margin: -0.5rem;
      border-radius: 0;
    }
    
    h2 {
      font-size: 1.25rem;
      margin-bottom: 1rem;
    }
    
    input, select {
      font-size: 1rem;
      padding: 0.75rem;
    }
    
    label {
      font-size: 0.9rem;
    }
  }
</style>
