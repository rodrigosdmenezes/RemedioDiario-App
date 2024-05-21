import React, { useState } from "react";
import {
  View,
  Text,
  StyleSheet,
  TextInput,
  TouchableOpacity,
  Alert
} from "react-native";
import Icon from "react-native-vector-icons/FontAwesome";
import { useNavigation } from "@react-navigation/native";
import axios from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';

export default function SignIn() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const navigation = useNavigation();

  const handleSignIn = async () => {
    if (!email.trim() || !password.trim()) {
      Alert.alert("Erro", "Por favor, insira seu email e senha.");
      return;
    }

    try {
      const tokenResponse = await axios.post('http://localhost:5122/api/auth', {
        email: email,
        password: password
      });

      if (tokenResponse.status === 200 && tokenResponse.data && tokenResponse.data.token) {
        const token = tokenResponse.data.token;
        console.log("Token recebido:", token);

        await AsyncStorage.setItem('token', token);

        // Enviando o token no cabeçalho da solicitação para o back-end
        const userResponse = await axios.post(
          'http://localhost:5122/api/login',
          {
            email: email,
            password: password
          },
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );

        console.log("Resposta do usuário:", userResponse);

        if (userResponse.status === 200) {
          navigation.navigate("Menu");
        } else {
          Alert.alert("Erro", "Credenciais inválidas. Por favor, verifique seu email e senha.");
        }
      } else {
        Alert.alert("Erro", "Ocorreu um erro ao fazer login. Por favor, tente novamente.");
      }
    } catch (error) {
      console.log(error);
      Alert.alert("Erro", "Ocorreu um erro ao fazer login. Por favor, tente novamente.");
    }
  };

  const handleTogglePasswordVisibility = () => {
    setShowPassword(!showPassword);
  };

  return (
    <View style={styles.container}>
      <View style={styles.containerHeader}>
        <Text style={styles.message}>Bem-vindo(a)!</Text>
      </View>

      <View style={styles.containerForm}>
        <Text style={styles.title}>Email</Text>
        <TextInput
          placeholder="Digite seu email"
          style={styles.input}
          value={email}
          onChangeText={setEmail}
          keyboardType="email-address"
          autoCapitalize="none"
        />

        <Text style={styles.title}>Senha</Text>
        <View style={styles.passwordContainer}>
          <TextInput
            placeholder="Digite sua senha"
            style={[styles.input, { flex: 1 }]}
            value={password}
            onChangeText={setPassword}
            secureTextEntry={!showPassword}
          />
          <TouchableOpacity onPress={handleTogglePasswordVisibility}>
            <Icon
              name={showPassword ? "eye-slash" : "eye"}
              size={20}
              color="#777"
            />
          </TouchableOpacity>
        </View>

        <TouchableOpacity style={styles.button} onPress={handleSignIn}>
          <Text style={styles.buttonText}>Acessar</Text>
        </TouchableOpacity>

        <TouchableOpacity
          style={styles.buttomRegister}
          onPress={() => navigation.navigate("SignUp")}
        >
          <Text style={styles.registerText}>Não possui uma conta? Cadastre-se</Text>
        </TouchableOpacity>

        <TouchableOpacity
          style={styles.buttomRegister}
          onPress={() => navigation.navigate("Reset")}
        >
          <Text style={styles.registerText}>Resetar Senha</Text>
        </TouchableOpacity>
      </View>
    </View>
  );
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#38A69D",
  },
  containerHeader: {
    marginTop: "14%",
    marginBottom: "8%",
    paddingHorizontal: "5%",
  },
  message: {
    fontSize: 28,
    fontWeight: "bold",
    color: "#FFF",
    marginTop: 28,
  },
  containerForm: {
    backgroundColor: "#FFF",
    flex: 1,
    borderTopLeftRadius: 25,
    borderTopRightRadius: 25,
    paddingHorizontal: "5%",
    paddingTop: 20,
  },
  title: {
    fontSize: 20,
    marginTop: 20,
  },
  input: {
    borderBottomWidth: 1,
    height: 40,
    marginBottom: 12,
    fontSize: 16,
  },
  button: {
    backgroundColor: "#38A69D",
    width: "100%",
    borderRadius: 4,
    paddingVertical: 8,
    marginTop: 20,
    justifyContent: "center",
    alignItems: "center",
  },
  buttonText: {
    color: "#FFF",
    fontSize: 18,
    fontWeight: "bold",
  },
  buttomRegister: {
    marginTop: 14,
    alignSelf: "center",
  },
  registerText: {
    color: "#a1a1a1",
  },
  passwordContainer: {
    flexDirection: 'row',
    alignItems: 'center',
  },
});
