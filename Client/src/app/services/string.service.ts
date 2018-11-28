

export class StringService {
    public isNullOrWhitespace(input: string): boolean {
        if (typeof input === 'undefined' || input == null) {
          return true;
        }
        return input.replace(/\s/g, '').length < 1;
      }
}
