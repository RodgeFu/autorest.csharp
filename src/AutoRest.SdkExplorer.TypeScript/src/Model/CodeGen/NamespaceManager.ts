
export class NamespaceManager {
  public namespaces: string[] = [];

  constructor() { }

  public pushNamespace(namespace: string) {
    let ns = namespace.trim();
    if (ns.length > 0 && !this.namespaces.includes(ns))
    {
      this.namespaces.push(ns);
      this.sort();
    }
  }

  public pushNamespaceArray(namespace: string[]) {
    namespace.forEach(v => {
      let ns = v.trim();
      if (ns.length > 0 && !this.namespaces.includes(ns))
        this.namespaces.push(ns);
    });
    this.sort();
  }

  private sort() {
    this.namespaces.sort((a, b) => {
      if (a.startsWith("System"))
        a = "@" + a;
      if (b.startsWith("System"))
        b = "#" + b;
      return a > b ? 1 : -1;
    })
  }

  public toCode() {
    return this.namespaces.map(v => "using " + v +";").join("\n");
  }

  public removeNamespace(code: string) {
    this.namespaces.map(v=>v).reverse().forEach(ns => {
      code = code.replaceAll(`global::${ns}.`, '');
    })
    return code;
  }


}
