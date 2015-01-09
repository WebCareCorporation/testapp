define([], function ()
{
    if (window.cordova) {
        var db = window.sqlitePlugin.openDatabase({ name: "StudentDb.db" });

        var transaction = db.transaction(function (tx) {

            tx.executeSql('CREATE TABLE IF NOT EXISTS test_table (id integer primary key, data text, data_num integer)');

            // demonstrate PRAGMA:
            db.executeSql("pragma table_info (test_table);", [], function (res) {
                console.log("PRAGMA res: " + JSON.stringify(res));
            });


            tx.executeSql("INSERT INTO test_table (data, data_num) VALUES (?,?)", ["test", 100], function (tx, res) {
                console.log("insertId: " + res.insertId + " -- probably 1");
                console.log("rowsAffected: " + res.rowsAffected + " -- should be 1");

                db.transaction(function (tx) {
                    tx.executeSql("select count(id) as cnt from test_table;", [], function (tx, res) {
                        //     alert("res.rows.length: " + res.rows.length + " -- should be 1");
                        //   alert("res.rows.item(0).cnt: " + res.rows.item(0).cnt + " -- should be 1");

                    });
                });

            }, function (e) {
                console.log("ERROR: " + e.message);
            });
        });
    }
    return {
        init: function () {
           // transaction();
        }
    }
});