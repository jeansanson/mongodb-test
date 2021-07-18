import { Loan } from "./loan";

export class Book {

  id: string = '';

  title: string = '';

  author: string = '';

  loans: Loan[] | null = null;

}
