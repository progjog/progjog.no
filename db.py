import sqlite3

def get_sessions():
    cur = sqlite3.connect("dev.db")
    res = cur.execute("SELECT * FROM session")
    sessions = res.fetchall()
    return sessions


def setup():
    import sqlite3 
    with open("dev.sql", "r") as sql_file:
        sql_script = sql_file.read()
        cur = sqlite3.connect("dev.db")
        cur.executescript(sql_script)

if __name__ == "__main__":
    setup()
