import { Injectable } from "@angular/core";
import { DefaultStateMatcher } from "./custom-state-matcher";

@Injectable()
export class HelperForms {

  static getStateMatcher(): DefaultStateMatcher {
    return new DefaultStateMatcher();
  }

}
