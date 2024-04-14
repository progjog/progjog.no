from db import get_sessions
from flask import Flask, render_template

app = Flask(__name__)

@app.route("/")
def index():
    sessions = get_sessions()
    context = {"sessions": sessions}
    return render_template("index.html", context=context)


"""
*GET /training-session (dont need?)
GET /training-session/<id>

POST /training-session/ (Create)
PUT /training-session/<id> (Update)

DELETE /training-session
"""
