# check for input
if [ -z "$1" ]; then
    echo "Please provide a name for the migration"
    exit 1
fi

dotnet ef migrations add $1 --project src/
