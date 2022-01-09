import sys, re, itertools, json

ignored = ['.git', 'node_modules', 'bower_components', '.sass-cache', '.png', '.ico', '.mov']
api_key_min_entropy_ratio = 0.5
api_key_min_length = 7


def pairwise(iterable):
    "s -> (s0,s1), (s1,s2), (s2, s3), ..."
    a, b = itertools.tee(iterable)
    next(b, None)
    return zip(a, b)


def token_is_api_key(token):
    """
    Returns True if the token is an API key or password.
    """
    if len(token) < api_key_min_length:
        return (False, '')
    entropy = 0
    for a, b in pairwise(list(token)):
        if not ((str.islower(a)
                 and str.islower(b))
                or (str.isupper(a)
                    and str.isupper(b))
                or (str.isdigit(a)
                    and str.isdigit(b))):
            entropy += 1
    return (float(entropy) / len(token) > api_key_min_entropy_ratio, float(entropy) / len(token))


def line_contains_api_key(line):
    """
    Returns True if any token in the line contains an API key or password.
    """
    for token in re.findall(r"[\w]+", line):
        result = token_is_api_key(token)
        if result[0]:
            return (True, result[1])
    return (False, '')


def scan_file(path_to_file):
    """
    Prints out lines in the specified file that probably contain an API key or
    password.
    """
    
    result_lines = list()
    number = 1
    for line in path_to_file.splitlines():
        result = line_contains_api_key(line)
        if result[0]:
            #print('Line ' + str(number) + ' : Entropy ' + str(result[1]) + "\n")
            #print(line)
            temp = dict()
            temp["line"] = line;
            temp["line_num"] = str(number);
            result_lines.append(temp)
        number += 1
    data = dict()
    data["data"] = result_lines
    print(json.dumps(data, indent=4))


scan_file(sys.argv[1])
