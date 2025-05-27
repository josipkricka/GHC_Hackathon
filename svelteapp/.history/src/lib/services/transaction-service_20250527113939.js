import { writable } from 'svelte/store';
import transactionsData from '../data/transactions.json';
import { Transaction } from '../models/transaction';

// Create a writable store with the initial transaction data
export const transactions = writable(transactionsData.map(t => Transaction.fromJson(t)));

// Service functions for CRUD operations
export const transactionService = {
  /**
   * Get all transactions
   */
  getAll() {
    // In a real API, this would be a fetch call
    return new Promise((resolve) => {
      setTimeout(() => {
        let currentTransactions;
        transactions.subscribe(value => {
          currentTransactions = value;
        })();
        resolve(currentTransactions);
      }, 100);
    });
  },

  /**
   * Get a transaction by ID
   */
  getById(transNum) {
    // In a real API, this would be a fetch call
    return new Promise((resolve) => {
      setTimeout(() => {
        let currentTransactions;
        transactions.subscribe(value => {
          currentTransactions = value;
        })();
        const transaction = currentTransactions.find(t => t.trans_num === transNum);
        resolve(transaction);
      }, 100);
    });
  },

  /**
   * Add a new transaction
   */
  create(transaction) {
    // In a real API, this would be a POST request
    return new Promise((resolve) => {
      setTimeout(() => {
        transactions.update(items => {
          const newTransaction = Transaction.fromJson(transaction);
          return [...items, newTransaction];
        });
        resolve(transaction);
      }, 100);
    });
  },

  /**
   * Update an existing transaction
   */
  update(transaction) {
    // In a real API, this would be a PUT request
    return new Promise((resolve) => {
      setTimeout(() => {
        transactions.update(items => {
          const index = items.findIndex(t => t.trans_num === transaction.trans_num);
          if (index !== -1) {
            const updatedTransaction = Transaction.fromJson(transaction);
            const updatedItems = [...items];
            updatedItems[index] = updatedTransaction;
            return updatedItems;
          }
          return items;
        });
        resolve(transaction);
      }, 100);
    });
  },

  /**
   * Delete a transaction by ID
   */
  delete(transNum) {
    // In a real API, this would be a DELETE request
    return new Promise((resolve) => {
      setTimeout(() => {
        transactions.update(items => items.filter(t => t.trans_num !== transNum));
        resolve({ success: true });
      }, 100);
    });
  }
};
