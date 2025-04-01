
import re


file_path = r'C:\Users\hari4\Documents\TestReport.txt'
with open('Input.txt', 'r') as file:

    contents = file.read()

    lines = contents.splitlines()
    sum = 0

    replacements = {
        "one": "1",
        "two": "2",
        "three": "3",
        "four": "4",
        "five": "5",
        "six": "6",
        "seven": "7",
        "eight": "8",
        "eigh": "8",
        "igh": "8",
        "nine": "9"
    }

    def processString(line):
        orginalLine = line
        for word, replacement in replacements.items():
            line = line.replace(word, replacement)
        return line

    for eachLine in lines:
        print(processString(eachLine))
        numbers = re.findall(r'\d+', processString(eachLine))
        if len(numbers) >= 1:
            firstNumber = numbers[0][0]
            lastNumberWhole = numbers[len(numbers) - 1]
            lastNumber = lastNumberWhole[len(lastNumberWhole)- 1]
            sum = sum + (int(firstNumber) * 10 + int(lastNumber))
    
    print(sum)



   