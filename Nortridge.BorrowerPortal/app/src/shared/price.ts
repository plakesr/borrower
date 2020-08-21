export default class Price {
    static locale: string = 'en-US';

    private constructor(public value: number) { }

    public static of(source: string | undefined): Price
    public static of(source: number | undefined): Price
    public static of(source: string | undefined | number): Price {
        if (typeof source === 'string') {
            const _value = source.trim();
            return new Price(_value.length == 0 ? 0 : parseFloat(_value.replace(/,/g, '')));
        } else if (typeof source === 'number') {
            return new Price(source);
        } else {
            return new Price(0);
        }
    }

    public add = (source: number | Price): Price =>
        this.map(source, (_value, _source) => _value + _source);

    public subtract = (source: number | Price): Price =>
        this.map(source, (_value, _source) => _value - _source);

    public multiply = (source: number | Price): Price =>
        this.map(source, (_value, _source) => _value * _source);

    public toShortString = (): string => this.value.toLocaleString(Price.locale, {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2
    });

    public toLongString = (): string => this.value.toLocaleString(Price.locale, {
        minimumFractionDigits: 5,
        maximumFractionDigits: 5
    });

    public isNaN = (): boolean => isNaN(this.value);

    public isNumber = (): boolean => !this.isNaN();

    private map(source: number | Price, func: (value: number, source: number) => number) {
        const _source = typeof source === 'number' ? source : source.value;
        return new Price(func(this.value, _source));
    }
}
