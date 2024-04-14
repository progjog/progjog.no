import sqlite3
import settings


def get_sessions():
    cur = sqlite3.connect(settings.DATABASE)
    res = cur.execute("SELECT * FROM session")
    sessions = res.fetchall()
    cur.close()
    return sessions


def add_session(title, details, date):
    cur = sqlite3.connect(settings.DATABASE)
    cur.execute("INSERT INTO session (title, details, date) VALUES (?, ?, ?)", (title, details, date))
    cur.commit()
    cur.close()


def delete_session(id):
    cur = sqlite3.connect(settings.DATABASE)
    cur.execute("DELETE FROM session WHERE session.id = ?", id)
    cur.commit()
    cur.close()


def setup() -> None:
    import sqlite3 
    with open(settings.DATABASE_SCHEMA, "r") as sql_file:
        sql_script = sql_file.read()
        cur = sqlite3.connect(settings.DATABASE)
        cur.executescript(sql_script)
        cur.close()


if __name__ == "__main__":
    setup()
