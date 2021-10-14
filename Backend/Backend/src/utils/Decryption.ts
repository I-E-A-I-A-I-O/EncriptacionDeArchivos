import { RSA_PKCS1_PADDING } from "constants";
import crypto from "crypto";
import fs from "fs-extra";

export class Decryption
{
    private static privateKey = crypto.createPrivateKey(fs.readFileSync("utils/key/PrivateKey.pem"));

    public static DecryptAndSave(data: Buffer): Buffer {
        const encryptedKey = data.slice(0, 512);
        const iv = data.slice(512, 512 + 16);
        const encryptedData = data.slice(512 + 16, data.byteLength);
        const decryptedKey = crypto.privateDecrypt({ key: this.privateKey, padding: RSA_PKCS1_PADDING }, encryptedKey);
        const decipher = crypto.createDecipheriv("aes-256-cbc", decryptedKey, iv);
        let decrypted = Buffer.concat([decipher.update(encryptedData), decipher.final()])
        return decrypted;
    }
}
