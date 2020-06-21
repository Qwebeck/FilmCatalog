import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'truncateText'
})
export class TruncateTextPipe implements PipeTransform {
  private readonly ellipses  = "...";
  private readonly biggestWord = 50;

  transform(value: string, len: number): string {
    let truncated = value.slice(0, len + this.biggestWord);
    while ( truncated.length > len ) {
      let lastSpace = truncated.lastIndexOf(" ");
      if (lastSpace == -1 ) break;
      truncated = truncated.slice(0, lastSpace);
    }
    truncated.replace(/[!,.?]$/, '');
    return truncated + this.ellipses;
  }

}
