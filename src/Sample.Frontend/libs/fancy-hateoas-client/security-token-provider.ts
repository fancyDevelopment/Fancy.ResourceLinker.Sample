export abstract class SecurityTokenProvider {
    /** Retrieves the current security token. */
    abstract retrieveCurrentToken(): Promise<string>;
}
