package com.csan3.http_rest.service;

import java.io.File;

public class FileService {

    public static String readFolder(File folder) {
        File[] files = folder.listFiles();

        if (files == null) return null;
        if (files.length == 0) return null;

        StringBuilder response = new StringBuilder();
        for (int i = 0; i < files.length; i++) {
            response.append(files[i]);
            if (i != files.length - 1)
                response.append('\n');
        }

        return response.toString();
    }
}
