/**
 * Transaction model representation
 */
export class Transaction {
  constructor(
    trans_num = '',
    amount = 0,
    trans_date = '',
    trans_time = '',
    category = '',
    first = '',
    last = '',
    gender = '',
    dob = '',
    city = '',
    job = ''
  ) {
    this.trans_num = trans_num;
    this.amount = amount;
    this.trans_date = trans_date;
    this.trans_time = trans_time;
    this.category = category;
    this.first = first;
    this.last = last;
    this.gender = gender;
    this.dob = dob;
    this.city = city;
    this.job = job;
  }

  static fromJson(json) {
    return new Transaction(
      json.trans_num,
      json.amount,
      json.trans_date,
      json.trans_time,
      json.category,
      json.first,
      json.last,
      json.gender,
      json.dob,
      json.city,
      json.job
    );
  }
}
