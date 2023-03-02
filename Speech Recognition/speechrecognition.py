import speech_recognition as sr

file = open('outputs.txt','w')

while(1):
    r = sr.Recognizer()
    with sr.Microphone() as source:
        print("Say something!")
        audio = r.listen(source)

    # Speech recognition using Google Speech Recognition
    try:
        # for testing purposes, we're just using the default API key
        # to use another API key, use `r.recognize_google(audio, key="GOOGLE_SPEECH_RECOGNITION_API_KEY")`
        # instead of `r.recognize_google(audio)`
        outputs = r.recognize_google(audio, show_all=True)
        print("You said: ")
        print(outputs)
        if(outputs):
            outputs = outputs['alternative']
            outputs = [(d['transcript']).lower() for d in outputs]
            print(outputs)
        file.writelines(','.join(outputs))
        file.write('\n')
        file.flush()
    except sr.UnknownValueError:
        print("Google Speech Recognition could not understand audio")
    except sr.RequestError as e:
        print("Could not request results from Google Speech Recognition service; {0}".format(e))
file.close()