
export type EventSlimHandler<T> = (sender: any, arg: T) => void;

export class EventSlim<T> {

    private _handlers: Map<string, EventSlimHandler<T>> = new Map<string, EventSlimHandler<T>>();

    constructor() { }

    public registerHandler(key: string, handler: EventSlimHandler<T>, overwrite: boolean = true) {
        if (overwrite || !this._handlers.has(key))
            this._handlers.set(key, handler);
    }

    public unregisterHandler(key: string) {
        this._handlers.delete(key);
    }

    public trigger(sender: any, arg: T, pendingIfNoHandler: boolean = false) {
        this.triggerInternal(sender, arg);
    }

    private triggerInternal(sender: any, arg: T) {
        this._handlers.forEach((func) => func(sender, arg));
    }
}
