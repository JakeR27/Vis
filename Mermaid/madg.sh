#!/bin/bash
TITLE=$(cat <<EOF  
  __  __          _____   _____ 
 |  \/  |   /\   |  __ \ / ____|
 | \  / |  /  \  | |  | | |  __ 
 | |\/| | / /\ \ | |  | | | |_ |
 | |  | |/ ____ \| |__| | |__| |
 |_|  |_/_/    \_\_____/ \_____|
  Mermaid and Diagram Generator
      Jake Bailey-Saltmarsh
)
echo -e "${TITLE}\n"

# Gets all the MERMAID files

FILES=`find . -type f -name "*.mermaid"`
echo -e "Found the following files:\n"
echo "${FILES}"
echo ""

# Gets all hashes of previously proccessed MERMAID files

PREV=`cat previous_hashes.madg`
PREVC=`echo "${PREV}" | wc -l`
echo -e "Found ${PREVC} previous hashes"

# Clears temp file and previous hash file for tracking changed files
echo -n > previous_hashes.madg

# Calculate hashes for each MERMAID file and determine if processing should occur

for f in $FILES
do
	MATCHED=false
	HASH=`md5 -q $f`
	for ph in $PREV
	do
		#echo "Comparing ${HASH} == ${ph}"
		if [ "$HASH" == "$ph" ] ; then
			echo "Found match for ${f}"
			MATCHED=true
		fi
	done
	
	echo "${HASH}" >> previous_hashes.madg
	
	if [ $MATCHED == false ] ; then
		echo "No match for ${f}. Converting..."
		mmdc -i $f -o "${f}.png" -t neutral -b transparent -s 3
	fi
done


exit 1

echo "Converting to pngs..."
for f in $FILES
do
	echo "Converting $f"
	mmdc -i $f -o "${f}.png" -t neutral -b transparent -s 3
done
echo -e "\nDone."
