import { Injectable, Optional } from "@angular/core";
import { Http, Response } from "@angular/http";
import { Observable } from "rxjs";
import "rxjs/add/operator/map";

/**
 * provides auto-complete related utility functions
 */
@Injectable()
export class AutoComplete {

  public source: string;
  public pathToData: string;
  public listFormatter: (arg: any) => string;
  public httpMethod: string = "post";
  public exactMatch: boolean = false;
  public headers: any;
  public body: any;
  public filteredList: any[] = [];

  constructor( @Optional() private http: Http) {
    // ...
  }

  filter(list: any[], keyword: string, matchFormatted: boolean, httpMethod) {
    this.httpMethod = httpMethod;
    return list.filter(
      el => {
        let objStr = matchFormatted ? this.getFormattedListItem(el).toLowerCase() : JSON.stringify(el).toLowerCase();
        keyword = keyword.toLowerCase();
        //console.log(objStr, keyword, objStr.indexOf(keyword) !== -1);
        return objStr.indexOf(keyword) !== -1;
      }
    );
  }

  getFormattedListItem(data: any) {
    let formatted;
    let formatter = this.listFormatter || '(id) value';
    if (typeof formatter === 'function') {
      formatted = formatter.apply(this, [data]);
    } else if (typeof data !== 'object') {
      formatted = data;
    } else if (typeof formatter === 'string') {
      formatted = formatter;
      let matches = formatter.match(/[a-zA-Z0-9_\$]+/g);
      if (matches && typeof data !== 'string') {
        matches.forEach(key => {
          formatted = formatted.replace(key, data[key]);
        });
      }
    }
    return formatted;
  }

  /**
   * return remote data from the given source and options, and data path
   */
  getRemoteData(keyword: string, headers?: any): Observable<Response> {
    if (typeof this.source !== 'string') {
      throw "Invalid type of source, must be a string. e.g. https://bridgeportclaims.com?q=:my_keyword";
    } else if (!this.http) {
      throw "Http is required.";
    }
    let matches = this.source.match(/:[a-zA-Z_]+/);
    if (matches === null) {
      throw "Replacement word is missing.";
    }

    let data: any = {};
    let replacementWord = matches[0];
    let url = this.source.replace(replacementWord, keyword);
    let searchText = '';
    var regex = new RegExp(/[ ,\-;:+]/, "gi"),
      results = keyword.replace(regex, "|"),
      arr = results.split("|");
    for (var i = 0; i < arr.length; i++) {
      if (arr[i].length > 0) {
        searchText += arr[i] + "|";
      }
    }
    if (url.indexOf('claim-search') > -1) {
      data.exactMatch = this.exactMatch;
      data.delimiter = "|";
      data.searchText = searchText.substr(0, searchText.length - 1);
      url = url.substr(0, url.indexOf('?'));
    }
    let httpRequest = this.httpMethod == "post" ? this.http.post(url, data, { headers: headers }) : this.http.get(url, { headers: this.headers });
    return httpRequest
      .map(resp => resp.json())
      .map(resp => {
        let list = resp.data || resp;

        if (this.pathToData) {
          let paths = this.pathToData.split(".");
          paths.forEach(prop => list = list[prop]);
        }
        this.filteredList = list;
        return list;
      });
  };
}