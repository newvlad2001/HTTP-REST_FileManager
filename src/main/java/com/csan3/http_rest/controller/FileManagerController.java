package com.csan3.http_rest.controller;

import com.csan3.http_rest.service.FileService;
import org.apache.tomcat.util.http.fileupload.FileUtils;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.io.*;
import java.nio.file.Files;
import java.nio.file.StandardCopyOption;
import java.nio.file.StandardOpenOption;

@RestController
public class FileManagerController {

    private final String rootFolderName = "RemoteStorage\\";

    @GetMapping(value = "/showStorage")
    public ResponseEntity<String> read() {
        File rootDir = new File(rootFolderName);
        if (!rootDir.exists()) {
            if (!rootDir.mkdir()) {
                return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
            }
        }

        String response = FileService.readFolder(rootDir);
        return new ResponseEntity<>(response, HttpStatus.OK);
    }

    @GetMapping(value = "/open")
    public ResponseEntity<String> read(@RequestParam(name = "file") String filename) {
        //System.out.println(filename);
        File file = new File(rootFolderName + filename);
        if (!file.exists()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        HttpHeaders headers = new HttpHeaders();
        String response = null;
        if (file.isDirectory()) {
            response = FileService.readFolder(file);
            headers.add("isDir", "true");
        } else {
            try {
                response = new String(Files.readAllBytes(file.toPath()));
                headers.add("isDir", "false");
            } catch (IOException e) {
                return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
            }
        }

        return ResponseEntity.ok().headers(headers).body(response);
    }

    @PutMapping(value = "/createFile")
    public ResponseEntity<String> createFile(@RequestParam(name = "file") String filename, @RequestBody(required = false) String content) {
        File file = new File(rootFolderName + filename);
        file.getParentFile().mkdirs();
        try (BufferedWriter writer = new BufferedWriter(new FileWriter(file))) {
            if (!file.exists()) {
                if (!file.createNewFile()) {
                    return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
                }
            }
            if (content != null) {
                writer.write(content);
            }
        } catch (IOException e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }
        return new ResponseEntity<>(HttpStatus.OK);
    }

    @PutMapping(value = "/createDir")
    public ResponseEntity<String> createDir(@RequestParam(name = "dirname") String dirname, @RequestBody(required = false) String content) {
        File file = new File(rootFolderName + dirname);
        //System.out.println(dirname);
        if (file.exists()) {
            try {
                FileUtils.deleteDirectory(file);
            } catch (IOException e) {
                return new ResponseEntity<>(HttpStatus.FORBIDDEN);
            }
        }

        file.getParentFile().mkdirs();

        if (!file.mkdir()) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }

        return new ResponseEntity<>(HttpStatus.OK);
    }

    @PutMapping(value = "/copy")
    public ResponseEntity<String> createDir(@RequestParam(name = "deleteSrc") boolean deleteSrc, @RequestParam(name = "src") String src, @RequestParam(name = "dst") String dst, @RequestBody(required = false) String content) {
        File srcFile = new File(rootFolderName + src);
        File dstFile = new File(rootFolderName + dst);
        //System.out.println(dirname);
        if (srcFile.exists()) {
            try {
                Files.copy(srcFile.toPath(), dstFile.toPath(), StandardCopyOption.REPLACE_EXISTING);
            } catch (IOException e) {
                return new ResponseEntity<>(HttpStatus.FORBIDDEN);
            }
        }

        if (deleteSrc) {
            if (srcFile.isDirectory()) {
                try {
                    FileUtils.deleteDirectory(srcFile);
                } catch (IOException e) {
                    return new ResponseEntity<>(HttpStatus.FORBIDDEN);
                }
            } else {
                if (!srcFile.delete()) {
                    return new ResponseEntity<>(HttpStatus.FORBIDDEN);
                }
            }
        }

        return new ResponseEntity<>(HttpStatus.OK);
    }

    @PostMapping(value = "/appendFile")
    public ResponseEntity<String> create(@RequestParam(value = "file") String filename, @RequestBody String content) {
        File file = new File(rootFolderName + filename);
        if (!file.exists()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        if (file.isDirectory()) {
            return new ResponseEntity<>(HttpStatus.BAD_REQUEST);
        }
        try {
            Files.write(file.toPath(), content.getBytes(), StandardOpenOption.APPEND);
        } catch (IOException e) {
            return new ResponseEntity<>(HttpStatus.INTERNAL_SERVER_ERROR);
        }

        return new ResponseEntity<>(HttpStatus.OK);
    }

    @DeleteMapping(value = "/delete")
    public ResponseEntity<String> delete(@RequestParam(name = "file") String filename) {
        File file = new File(rootFolderName + filename);
        if (!file.exists()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        if (file.isDirectory()) {
            try {
                FileUtils.deleteDirectory(file);
            } catch (IOException e) {
                return new ResponseEntity<>(HttpStatus.FORBIDDEN);
            }
        } else {
            if (!file.delete()) {
                return new ResponseEntity<>(HttpStatus.NOT_MODIFIED);
            }
        }
        return new ResponseEntity<>(HttpStatus.OK);
    }
}
