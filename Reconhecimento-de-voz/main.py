
import random
import speech_recognition as sr  # Funcao responsavel por ouvir e reconhecer a fala


def ouvir_microfone():  # Habilita o microfone
    microfone = sr.Recognizer()
    with sr.Microphone() as source:
        microfone.adjust_for_ambient_noise(source)  # Chama a funcao de reducao de ruido
        print("Diga alguma coisa: ")  # Avisa ao usuario que esta pronto para ouvir
        audio = microfone.listen(source)  # Armazena a informacao de audio
    try:
        frase = str(microfone.recognize_google(audio, language='pt-BR'))  # Passa o audio para o reconhecedor de padroes
        return frase # Após alguns segundos, retorna a frase falada
    except sr.UnkownValueError:
        return "Não entendi"  # Caso nao tenha reconhecido o padrao de fala, exibe esta mensagem


def BD_Dialogo(nome = ""):
    nome = " " + nome if nome else ""
    Dialogo = {
        "Saudacoes": {
            "Informal": [
                [f"Beleza?", "T", (7, 8, 14, 15, 16, 18, 20, 21), (1, 2, 3)],  # 0
                [f"Tudo bem{nome}?", "T", (14, 15, 16, 21, 7, 8), (6)],  # 1
                [f"Tudo bom{nome}?", "T", (14, 15, 16, 21, 7, 8), (6)],    # 2
                [f"Tudo legal{nome}?", "T", (14, 15, 16, 21, 7, 8), (6)],  # 3
                [f"Tudo beleza{nome}?", "T", (14, 15, 16, 21, 7, 8), (6)],  # 4
                [f"Cara! Beleza?", "S", (16), (6)],  # 5
                [f"Estava com saudade!", "T", (1, 2, 3, 4, 7, 8), ()],  # 6
                [f"Como vai{nome}?", "T", (), ()],  # 7
                [f"Como está{nome}?", "T", (), ()],  # 8
                [f"Tudo tranquilo{nome}?", "T", (), ()],  # 9
                [f"Tudo ótimo{nome}", "T", (), ()],  # 10
                [f"Tudo joia{nome}", "T", (), ()],  # 11
                [f"Tudo beleza{nome}", "T", (), ()],  # 12
                [f"Tudo numa Boa{nome}", "T", (), ()],  # 13
                [f"E aí pessoal!", "P", (), ()],  # 14
                [f"E aí galera!", "P", (), ()],  # 15
                [f"E aí{nome}!", "T", (), ()],  # 16
                [f"Oi{nome}, tudo certo?", "S", (), ()],  # 17
                [f"Fala aí, cara!", "S", (), ()],  # 18
                [f"Beleza, brother?", "S", (), ()],  # 19
                [f"Qual é{nome}?", "T", (), ()],  # 20
                [f"Opa!", "T", (), ()],  # 21
            ],

            "Formal": [
                [f"Oi{nome}!", "S", (), ()],
                [f"Olá{nome}", "S", (), ()],
                [f"Ei, como vai", "P", (), ()],
                [f"Ola, como vocês estão", "P", (), ()],
                [f"Tudo bem?", "T", (), ()],
                [f"Tudo bom?", "T", (), ()],
            ],

            "Temporal_Manha": [
                [f"Bom dia{nome}", "S", (), ()],
            ],

            "Temporal_Tarde": [
                [f"Boa tarde{nome}", "S", (), ()],
            ],

            "Temporal_Noite": [
                [f"Boa noite{nome}", "S", (), ()],
            ]
        },

        "Agradecimento": {
            "Informal": [
                [f"Valew{nome}", "S", (), ()],
                [f"Te devo uma{nome}", "S", (), ()],
                [f"Te devo essa{nome}", "S", (), ()],
                [f"Vlw galera", "P", (), ()],
            ],

            "Formal": [
                [f"Obrigado{nome}", "S", (), ()],
                [f"Muito obrigado{nome}", "S", (), ()],
                [f"Agradecido", "T", (), ()],
                [f"Obrigado Pessoal", "P", (), ()],
            ],
        },

        "Despedida": {
            "Informal": [
                [f"Valeu{nome}!", "S", (), ()],
                [f"Já é{nome}!", "S", (), ()],
                [f'De boa!', "T", (), ()],
                [f"Tranquilo!", "T", (), ()],
                [f"Falow pessoal", "P", (), ()],
            ],

            "Formal": [
                [f"Até Logo{nome}", "S", (), ()],
                [f"Até Breve{nome}", "S", (), ()],
                [f"Adeus", "T", (), ()],
                [f"Tchau", "T", (), ()],
                [f"Tchall galera", "P", (), ()],
                [f"A Gente Se Ve{nome}", "S", (), ()],
            ],
        },
    }
    return Dialogo


def Ouvir_usuario():
    '''
    Utiliza funções de reconhecimento de voz e um dicionário de diálogos para retornar dados processados a partir
    da fala do usuário
    :return: Dicionário de respostas: {index: resposta}
             Cada resposta com 7 informações: [assunto, formalidade, frase, tipo, index, index_prefixos, index_sufixos]
    '''
    nome_maquina = "Jarvis"
    carac_proibidas = ["?", "!", ".", ";", ","]
    voz = ouvir_microfone()
    prop_dialogo = {}
    for assunto_titulo, assunto_modo in BD_Dialogo().items():
        for modo, dialogo in assunto_modo.items():
            for frase, tipo, prefixo, sufixo in dialogo:
                frase_limpa = frase
                for c in carac_proibidas:
                    frase_limpa = frase_limpa.replace(c, "")
                if frase_limpa.upper() in voz.upper():
                    if (nome_maquina.upper() in voz.upper() and len(voz) < len(frase_limpa) + len(nome_maquina) + 15) \
                    or (nome_maquina.upper() not in voz.upper() and len(voz) < len(frase_limpa) * 1.3):
                        prop_dialogo[len(prop_dialogo)] = [assunto_titulo, modo, frase, tipo,
                        dialogo.index([frase, tipo, prefixo, sufixo]), prefixo, sufixo]

    print(voz)
    return prop_dialogo


def Responder_usuario(nome="", propriedades={}):
    respostas = []
    for p in propriedades.values():
        assunto = p[0]
        formalidade = p[1]
        tipo = p[3]
        prefixos_id = p[5]
        sufixos_id = p[6]
        frases = BD_Dialogo(nome)[assunto][formalidade]
        frases_anonimas = BD_Dialogo()[assunto][formalidade]

        prefixos_comp = []
        sufixos_comp = []
        frases_comp = []
        for n, f in enumerate(frases_anonimas):
            if int(n) in prefixos_id:
                prefixos_comp.append(f[0])
            if int(n) in sufixos_id:
                sufixos_comp.append(f[0])
        for f in frases:
            if tipo == "P" and f[1] == tipo:
                frases_comp.append(f[0])
            elif tipo == "S" and f[1] != "P":
                frases_comp.append(f[0])
            elif tipo == "T" and f[1] != "P":
                frases_comp.append(f[0])

        index_prefixo = random.randint(0, len(prefixos_comp) - 1) if prefixos_id else False
        index_corpo = random.randint(0, len(frases_comp) - 1)
        index_sufixo = random.randint(0, len(sufixos_comp) - 1) if sufixos_id else False

        index_prefixo = index_prefixo if index_prefixo and index_corpo != index_prefixo else False
        index_sufixo = index_sufixo if index_sufixo and index_sufixo != index_prefixo else False

        resposta_corpo = frases_comp[index_corpo]
        resposta_prefixo = prefixos_comp[index_prefixo] + ", " if index_prefixo else ""
        resposta_sufixo = ", " + sufixos_comp[index_sufixo] if index_sufixo else ""

        respostas.append(resposta_prefixo + resposta_corpo + resposta_sufixo)

    return respostas[random.randint(0, len(respostas) - 1)] if respostas else ""


print(Responder_usuario("Laion", Ouvir_usuario()))