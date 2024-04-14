from db import delete_session, get_sessions, add_session
from flask import Flask, render_template, redirect, url_for, request

app = Flask(__name__)

@app.route("/")
def index():
    sessions = get_sessions()
    context = {"sessions": sessions}
    return render_template("index.html", context=context)


@app.route("/session/new", methods=["POST"])
def session_new():
    # TODO: add flash messages
    # TODO: add auth
    if request.method == "POST":
        form = request.form

        title = form["title"]
        date = form["date"]
        details = form["details"]

        add_session(title, details, date)

    return redirect(url_for("index"))


# DELETE /training-session
@app.route("/session/delete/<id>", methods=["GET"])
def session_delete(id):
    delete_session(id)
    return redirect(url_for("index"))


"""
*GET /training-session (dont need?)
GET /training-session/<id>

POST /training-session/ (Create)
PUT /training-session/<id> (Update)

"""
