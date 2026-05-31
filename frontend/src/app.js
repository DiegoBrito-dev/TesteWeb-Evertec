const { useCallback, useEffect, useMemo, useState } = React;

const apiBaseUrl = "http://localhost:5196";

function App() {
  const [activeView, setActiveView] = useState("list");
  const [refreshToken, setRefreshToken] = useState(0);

  const handleCreated = () => {
    setRefreshToken((value) => value + 1);
    setActiveView("list");
  };

  return React.createElement(
    "div",
    { className: "app-shell" },
    React.createElement(Header, { activeView, onNavigate: setActiveView }),
    React.createElement(
      "main",
      { className: "content" },
      activeView === "list"
        ? React.createElement(ListaPontosTuristicos, { refreshToken })
        : React.createElement(FormularioPontoTuristico, { onCreated: handleCreated })
    )
  );
}

function Header({ activeView, onNavigate }) {
  return React.createElement(
    "header",
    { className: "topbar" },
    React.createElement(
      "div",
      { className: "topbar-inner" },
      React.createElement(
        "div",
        { className: "brand" },
        React.createElement("strong", null, "Pontos Turisticos"),
        React.createElement("span", null, "Cadastro e consulta por localizacao")
      ),
      React.createElement(
        "nav",
        { className: "nav" },
        React.createElement(
          "button",
          {
            className: activeView === "list" ? "active" : "",
            onClick: () => onNavigate("list")
          },
          "Listagem"
        ),
        React.createElement(
          "button",
          {
            className: activeView === "form" ? "active" : "",
            onClick: () => onNavigate("form")
          },
          "Cadastro"
        )
      )
    )
  );
}

class ListaPontosTuristicos extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      data: null,
      selected: null,
      search: "",
      page: 1,
      loading: true,
      error: ""
    };

    this.searchTimeout = null;
  }

  componentDidMount() {
    this.carregarPontosTuristicos();
  }

  componentDidUpdate(previousProps, previousState) {
    if (
      previousProps.refreshToken !== this.props.refreshToken ||
      previousState.page !== this.state.page
    ) {
      this.carregarPontosTuristicos();
    }
  }

  componentWillUnmount() {
    window.clearTimeout(this.searchTimeout);
  }

  carregarPontosTuristicos = async () => {
    this.setState({ loading: true, error: "" });

    const query = new URLSearchParams({
      pagina: this.state.page,
      tamanhoPagina: 8
    });

    if (this.state.search.trim()) {
      query.set("busca", this.state.search.trim());
    }

    try {
      const response = await fetch(`${apiBaseUrl}/api/pontos-turisticos?${query}`);

      if (!response.ok) {
        throw new Error("Nao foi possivel carregar os pontos turisticos.");
      }

      const data = await response.json();
      const selected = data.itens.some((item) => item.id === this.state.selected?.id)
        ? this.state.selected
        : null;

      this.setState({ data, selected, loading: false });
    } catch (error) {
      this.setState({ error: error.message, loading: false });
    }
  };

  carregarDetalhe = async (id) => {
    this.setState({ error: "" });

    try {
      const response = await fetch(`${apiBaseUrl}/api/pontos-turisticos/${id}`);

      if (!response.ok) {
        throw new Error("Nao foi possivel abrir o ponto turistico selecionado.");
      }

      this.setState({ selected: await response.json() });
    } catch (error) {
      this.setState({ error: error.message });
    }
  };

  handleSearch = (event) => {
    const search = event.target.value;
    window.clearTimeout(this.searchTimeout);

    this.setState({ search, page: 1 }, () => {
      this.searchTimeout = window.setTimeout(this.carregarPontosTuristicos, 280);
    });
  };

  render() {
    const { data, selected, search, page, loading, error } = this.state;
    const items = data?.itens ?? [];
    const totalPages = data?.totalPaginas ?? 0;

    return React.createElement(
      React.Fragment,
      null,
      React.createElement(
        "div",
        { className: "toolbar" },
        React.createElement("input", {
          className: "search",
          value: search,
          onChange: this.handleSearch,
          placeholder: "Buscar por nome, descricao ou localizacao"
        }),
        React.createElement(
          "div",
          { className: "summary" },
          data ? `${data.totalItens} registro(s)` : "Carregando"
        )
      ),
      error ? React.createElement("div", { className: "error" }, error) : null,
      React.createElement(
        "div",
        { className: "list-grid" },
        React.createElement(
          "section",
          null,
          loading
            ? React.createElement("div", { className: "empty" }, "Carregando pontos turisticos...")
            : items.length === 0
              ? React.createElement("div", { className: "empty" }, "Nenhum ponto turistico encontrado.")
              : React.createElement(
                  "div",
                  { className: "spots" },
                  items.map((item) =>
                    React.createElement(
                      "button",
                      {
                        key: item.id,
                        className: `spot-row ${selected?.id === item.id ? "selected" : ""}`,
                        onClick: () => this.carregarDetalhe(item.id)
                      },
                      React.createElement("strong", null, item.nome),
                      React.createElement("span", null, `${item.localizacao} - ${item.cidade}/${item.estado}`)
                    )
                  )
                ),
          React.createElement(
            "div",
            { className: "pager" },
            React.createElement(
              "button",
              {
                className: "secondary",
                disabled: page <= 1,
                onClick: () => this.setState({ page: page - 1 })
              },
              "Anterior"
            ),
            React.createElement("span", null, totalPages === 0 ? "Pagina 0 de 0" : `Pagina ${page} de ${totalPages}`),
            React.createElement(
              "button",
              {
                className: "secondary",
                disabled: totalPages === 0 || page >= totalPages,
                onClick: () => this.setState({ page: page + 1 })
              },
              "Proxima"
            )
          )
        ),
        React.createElement(DetalhePontoTuristico, { pontoTuristico: selected })
      )
    );
  }
}

function DetalhePontoTuristico({ pontoTuristico }) {
  if (!pontoTuristico) {
    return React.createElement(
      "aside",
      { className: "detail-panel" },
      React.createElement("h2", null, "Detalhe"),
      React.createElement("p", { className: "hint" }, "Selecione um ponto turistico na listagem.")
    );
  }

  return React.createElement(
    "aside",
    { className: "detail-panel" },
    React.createElement("h2", null, pontoTuristico.nome),
    React.createElement(
      "dl",
      null,
      React.createElement("div", null, React.createElement("dt", null, "Descricao"), React.createElement("dd", null, pontoTuristico.descricao)),
      React.createElement("div", null, React.createElement("dt", null, "Localizacao"), React.createElement("dd", null, pontoTuristico.localizacao)),
      React.createElement("div", null, React.createElement("dt", null, "Cidade/Estado"), React.createElement("dd", null, `${pontoTuristico.cidade}/${pontoTuristico.estado}`))
    )
  );
}

function FormularioPontoTuristico({ onCreated }) {
  const [states, setStates] = useState([]);
  const [form, setForm] = useState({
    nome: "",
    descricao: "",
    localizacao: "",
    cidade: "",
    estado: ""
  });
  const [status, setStatus] = useState("");
  const [error, setError] = useState("");
  const remainingCharacters = useMemo(() => 100 - form.descricao.length, [form.descricao]);

  useEffect(() => {
    fetch(`${apiBaseUrl}/api/estados`)
      .then((response) => response.json())
      .then(setStates)
      .catch(() => setError("Nao foi possivel carregar os estados."));
  }, []);

  const updateField = useCallback((field, value) => {
    setForm((current) => ({ ...current, [field]: value }));
  }, []);

  const submit = async (event) => {
    event.preventDefault();
    setStatus("");
    setError("");

    try {
      const response = await fetch(`${apiBaseUrl}/api/pontos-turisticos`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(form)
      });

      if (!response.ok) {
        const problem = await response.json().catch(() => null);
        throw new Error(problem?.detail ?? "Nao foi possivel cadastrar o ponto turistico.");
      }

      setStatus("Ponto turistico cadastrado com sucesso.");
      setForm({ nome: "", descricao: "", localizacao: "", cidade: "", estado: "" });
      window.setTimeout(onCreated, 450);
    } catch (requestError) {
      setError(requestError.message);
    }
  };

  return React.createElement(
    "section",
    { className: "form-panel" },
    React.createElement("h1", null, "Cadastrar ponto turistico"),
    status ? React.createElement("div", { className: "status" }, status) : null,
    error ? React.createElement("div", { className: "error" }, error) : null,
    React.createElement(
      "form",
      { className: "form-grid", onSubmit: submit },
      React.createElement(Field, {
        label: "Nome",
        value: form.nome,
        onChange: (value) => updateField("nome", value),
        maxLength: 120,
        required: true
      }),
      React.createElement(Field, {
        label: "Descricao",
        value: form.descricao,
        onChange: (value) => updateField("descricao", value),
        maxLength: 100,
        required: true,
        multiline: true,
        hint: `${remainingCharacters} caractere(s) restante(s)`
      }),
      React.createElement(Field, {
        label: "Localizacao",
        value: form.localizacao,
        onChange: (value) => updateField("localizacao", value),
        maxLength: 200,
        required: true
      }),
      React.createElement(
        "div",
        { className: "field-row" },
        React.createElement(Field, {
          label: "Cidade",
          value: form.cidade,
          onChange: (value) => updateField("cidade", value),
          maxLength: 120,
          required: true
        }),
        React.createElement(
          "label",
          { className: "field" },
          React.createElement("span", null, "Estado"),
          React.createElement(
            "select",
            {
              value: form.estado,
              onChange: (event) => updateField("estado", event.target.value),
              required: true
            },
            React.createElement("option", { value: "" }, "Selecione"),
            states.map((state) =>
              React.createElement("option", { key: state.codigo, value: state.codigo }, state.codigo)
            )
          )
        )
      ),
      React.createElement(
        "div",
        { className: "actions" },
        React.createElement("button", { className: "primary", type: "submit" }, "Cadastrar")
      )
    )
  );
}

function Field({ label, value, onChange, maxLength, required, multiline, hint }) {
  return React.createElement(
    "label",
    { className: "field" },
    React.createElement("span", null, label),
    multiline
      ? React.createElement("textarea", {
          value,
          maxLength,
          required,
          onChange: (event) => onChange(event.target.value)
        })
      : React.createElement("input", {
          value,
          maxLength,
          required,
          onChange: (event) => onChange(event.target.value)
        }),
    hint ? React.createElement("span", { className: "hint" }, hint) : null
  );
}

ReactDOM.createRoot(document.getElementById("root")).render(React.createElement(App));
