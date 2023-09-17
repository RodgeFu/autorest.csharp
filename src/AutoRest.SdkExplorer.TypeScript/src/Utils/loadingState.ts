import moment from "moment";
import { MessageItem } from "./messageItem";
import { EventSlim } from "./EventSlim";

export class LoadingState {
  public state: "none" | "loading" | "loaded" | "error" = "none";
  public lastModifiedTime = moment.utc();

  public setToLoading() {
    this.state = 'loading';
    this.messageItem = undefined;
    this.lastModifiedTime = moment.utc();
    this.onLoading.trigger(this);
  }

  public setToLoaded() {
    this.state = 'loaded';
    this.messageItem = undefined;
    this.lastModifiedTime = moment.utc();
    this.onLoaded.trigger(this);
  }

  public setToError(msg: string) {
    this.state = 'error';
    this.lastModifiedTime = moment.utc();
    this._messageItem = new MessageItem(msg, "error");
    this.onError.trigger(this, this.messageItem!);
  }

  public isLoading() {
    return this.state === 'loading';
  }

  public isLoaded() {
    return this.state === 'loaded';
  }

  public isError() {
    return this.state === 'error';
  }

  public get message(): string | undefined {
    return this.messageItem ? this.messageItem.message : undefined;
  }

  private _messageItem?: MessageItem;
  public get messageItem(): MessageItem | undefined {
    return this._messageItem;
  }
  private set messageItem(value: MessageItem | undefined) {
    this._messageItem = value;
  }

  public reset() {
    this.state = 'none';
    this.messageItem = undefined;
    this.lastModifiedTime = moment.utc();
  }

  public triggerOnceNowOrWhenLoaded(key: string, func: () => void) {
    if (this.isLoaded()) {
      func();
    }
    else {
      this.onLoaded.registerHandler(key, () => {
        func();
        this.onLoaded.unregisterHandler(key);
      });
    }
  }

  public onLoaded: EventSlim<void> = new EventSlim<void>();
  public onLoading: EventSlim<void> = new EventSlim<void>();
  public onError: EventSlim<MessageItem> = new EventSlim<MessageItem>();

}
