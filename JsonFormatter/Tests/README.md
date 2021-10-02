Test scenarios:
+--+----------------------------------------------+-----------------------------------------------------------------
 # | Command Line                                 | Expected Result
+--+----------------------------------------------+-----------------------------------------------------------------
 1 | JsonFormatter                                | Output usage message
 2 | JsonFormatter sample.json sortbyid.table     | Success case
 3 | JsonFormatter sample2.json sortbyid.table    | Showcase of sorting by Id field
 4 | JsonFormatter badsample.json 123             | Showcase of input file that fails schema validation
+--+----------------------------------------------+-----------------------------------------------------------------
