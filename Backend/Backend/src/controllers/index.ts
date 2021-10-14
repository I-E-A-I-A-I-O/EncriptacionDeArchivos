import { Request, Response, NextFunction } from "express";
import { RequestError } from "../utils/Error";
import fs from "fs-extra";
import { Decryption } from "../utils/Decryption";

export const SayHello = (req: Request, res: Response, next: NextFunction) => {
    res.send("Hello world!");
};

export const ReceiveFile = async (req: Request, res: Response, next: NextFunction) => {
    const files = req.files as { [fieldname: string]: Express.Multer.File[] } | undefined;
    if (files == null) {
        const reqErr = new RequestError("No se recibieron archivos.");
        reqErr.statusCode = 400;
        next(reqErr);
        return;
    }

    try {
        if (!fs.existsSync("receivedFiles")) {
            fs.mkdirSync("receivedFiles");
        }

        const date = new Date().toISOString().replace(/:/g, "");

        fs.mkdirSync(`receivedFiles/${date}`)

        for (var i = 0; i < files.files.length; i++) {
            const decryptedFile = Decryption.DecryptAndSave(files.files[i].buffer);
            fs.outputFileSync(`receivedFiles/${date}/${files.files[i].originalname.replace(".enc", "")}`, decryptedFile);
        }

        res.sendStatus(200);
    } catch (e) {
        console.log(e);
        next(e);
    }
};
