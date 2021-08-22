This project use [ANTLRv4](https://github.com/antlr/antlr4) to parse TSql schema and build two compare file and one update schema.

# How to use:

Extract from SqlManagement the creation schema of the origin database and destination database.

## Compare
The compare feature creates two files that you can compare with WinMerge or other tool for comparing file. 
This feature discard from the schemas the objects that are equals and put in the files the creation statements that are different. They are ordered alphabetically.
You can choose the output directory where create the diff files and a suffix. 

The name of the diff files created are: 
* origin + suffix.extensionOfOriginFile
* destination + suffix.extensionOfOriginFile

An ErrorCompare.txt file is created with errors when parsing.
## Update schema
Update feature creates an update schema from origin to destination. 
You can choose the name of the database and the name of the update schema file.

An erro file is created with errors when parsing with name:
* UpdateSchemaFileName + _errorsUpdateSchema.extensionOfUpdateSchemaFileName 
